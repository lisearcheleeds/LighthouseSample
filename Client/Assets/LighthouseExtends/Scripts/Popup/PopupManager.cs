using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using VContainer;

namespace LighthouseExtends.Popup
{
    public sealed class PopupManager : IPopupManager
    {
        readonly IPopupAssetLoader popupAssetLoader;

        readonly List<(MainSceneId, List<IPopupData>)> popupDataSceneList = new();
        readonly List<IPopup> popupList = new();

        readonly Queue<PopupCommand> commandQueue = new();

        List<IPopupData> popupDataList;

        bool isProcessing;

        struct PopupCommand
        {
            public readonly Func<UniTask> Action;
            public readonly UniTaskCompletionSource Tcs;
            public readonly CancellationToken Token;

            public PopupCommand(Func<UniTask> action, UniTaskCompletionSource tcs, CancellationToken token)
            {
                Action = action;
                Tcs = tcs;
                Token = token;
            }
        }

        [Inject]
        public PopupManager(IPopupAssetLoader popupAssetLoader)
        {
            this.popupAssetLoader = popupAssetLoader;
        }

        UniTask IPopupManager.EnqueuePopup(IPopupData popupData, CancellationToken token)
        {
            return EnqueueCommand(() =>
            {
                EnqueuePopupCore(popupData);
                return UniTask.CompletedTask;
            }, token);
        }

        UniTask IPopupManager.OpenPopup(CancellationToken token)
        {
            return EnqueueCommand(() => OpenPopupCore(token), token);
        }

        UniTask IPopupManager.OpenPopup(IPopupData popupData, CancellationToken token)
        {
            return EnqueueCommand(async () =>
            {
                EnqueuePopupCore(popupData);
                await OpenPopupCore(token);
            }, token);
        }

        UniTask IPopupManager.ClosePopup(IPopupData popupData, CancellationToken token)
        {
            return EnqueueCommand(() => ClosePopupCore(popupData, token), token);
        }

        UniTask IPopupManager.ClosePopup(CancellationToken token)
        {
            return EnqueueCommand(() => ClosePopupCore(token), token);
        }

        UniTask IPopupManager.ClearAllPopup(CancellationToken token)
        {
            return EnqueueCommand(() => ClearAllPopupCore(token), token);
        }

        UniTask IPopupManager.ClearCurrentAllPopup(CancellationToken token)
        {
            return EnqueueCommand(() => ClearCurrentAllPopupCore(token), token);
        }

        UniTask IPopupManager.ResumePopupFromSceneId(MainSceneId mainSceneId, CancellationToken token)
        {
            return EnqueueCommand(async () =>
            {
                ResumePopupFromSceneIdCore(mainSceneId);

                if (popupDataList?.Any() ?? false)
                {
                    await OpenPopupCore(token);
                }
            }, token);
        }

        UniTask IPopupManager.SuspendPopupFromSceneId(MainSceneId mainSceneId, CancellationToken token)
        {
            return EnqueueCommand(async () =>
            {
                SuspendPopupFromSceneIdCore(mainSceneId);
                await ClearCurrentAllPopupCore(token);
            }, token);
        }

        UniTask EnqueueCommand(Func<UniTask> action, CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            commandQueue.Enqueue(new PopupCommand(action, tcs, token));

            if (!isProcessing)
            {
                isProcessing = true;
                CommandProcessLoop().Forget();
            }

            return tcs.Task;
        }

        async UniTaskVoid CommandProcessLoop()
        {
            while (commandQueue.Count > 0)
            {
                var command = commandQueue.Dequeue();

                if (command.Token.IsCancellationRequested)
                {
                    command.Tcs.TrySetCanceled(command.Token);
                    continue;
                }

                try
                {
                    await command.Action();
                    command.Tcs.TrySetResult();
                }
                catch (OperationCanceledException oce)
                {
                    command.Tcs.TrySetCanceled(oce.CancellationToken);
                }
                catch (Exception ex)
                {
                    command.Tcs.TrySetException(ex);
                }
            }

            isProcessing = false;
        }

        void EnqueuePopupCore(IPopupData popupData)
        {
            if (popupDataList == null)
            {
                popupDataList = new List<IPopupData>();
            }

            popupDataList.Add(popupData);
        }

        async UniTask OpenPopupCore(CancellationToken token)
        {
            var popupData = popupDataList?.LastOrDefault();
            if (popupData == null)
            {
                throw new InvalidOperationException($"Empty popup data");
            }

            var prevPopup = popupList.LastOrDefault();
            if (prevPopup?.PopupData == popupData)
            {
                throw new InvalidOperationException($"Duplicate open");
            }

            var popup = await popupAssetLoader.LoadPopupAsset(popupData.PopupAddress);
            popupList.Add(popup);

            await popup.OnInitialize();

            if (prevPopup != null)
            {
                if (!prevPopup.PopupData.IsKeepView)
                {
                    await prevPopup.OutAnimation();
                }

                await prevPopup.OnLeave();
            }

            await popup.OnEnter(popupData);
            await popup.InAnimation();
        }

        async UniTask ClosePopupCore(IPopupData popupData, CancellationToken token)
        {
            if (popupDataList == null || !popupDataList.Remove(popupData))
            {
                foreach (var popupDataScene in popupDataSceneList)
                {
                    if (popupDataScene.Item2.Remove(popupData))
                    {
                        break;
                    }
                }

                return;
            }

            if (!(popupList?.Any() ?? false))
            {
                return;
            }

            // If there is a popup view, remove it too.
            var target = popupList.FirstOrDefault(x => ReferenceEquals(x.PopupData, popupData));
            if (target == null)
            {
                return;
            }

            var isLast = ReferenceEquals(target, popupList[^1]);
            popupList.Remove(target);

            await target.OutAnimation();
            await target.OnLeave();
            target.Dispose();

            if (!isLast)
            {
                return;
            }

            var prevPopup = popupList.LastOrDefault();
            if (prevPopup == null)
            {
                return;
            }

            await prevPopup.OnEnter(prevPopup.PopupData);

            if (!prevPopup.PopupData.IsKeepView)
            {
                await prevPopup.InAnimation();
            }
        }

        async UniTask ClosePopupCore(CancellationToken token)
        {
            var lastPopupData = popupDataList?.LastOrDefault();
            if (lastPopupData == null || !popupDataList.Remove(lastPopupData))
            {
                return;
            }

            var currentPopup = popupList.LastOrDefault();
            if (currentPopup == null)
            {
                return;
            }

            popupList.Remove(currentPopup);

            await currentPopup.OutAnimation();
            await currentPopup.OnLeave();
            currentPopup.Dispose();

            var prevPopup = popupList.LastOrDefault();
            if (prevPopup == null)
            {
                return;
            }

            await prevPopup.OnEnter(prevPopup.PopupData);

            if (!prevPopup.PopupData.IsKeepView)
            {
                await prevPopup.InAnimation();
            }
        }

        UniTask ClearAllPopupCore(CancellationToken token)
        {
            popupDataSceneList.Clear();
            return ClearCurrentAllPopupCore(token);
        }

        async UniTask ClearCurrentAllPopupCore(CancellationToken token)
        {
            popupDataList?.Clear();

            var lastTarget = popupList.LastOrDefault();
            while (0 < popupList.Count)
            {
                var target = popupList[^1];
                var isLast = ReferenceEquals(target, lastTarget);
                popupList.RemoveAt(popupList.Count - 1);

                if (isLast || target.PopupData.IsKeepView)
                {
                    await target.OutAnimation();
                }

                await target.OnLeave();
                target.Dispose();
            }

            popupList.Clear();
        }

        void ResumePopupFromSceneIdCore(MainSceneId mainSceneId)
        {
            if (!popupDataSceneList.Any())
            {
                return;
            }

            var (lastMainSceneId, lastPopupDataList) = popupDataSceneList[^1];
            while (lastMainSceneId != mainSceneId)
            {
                popupDataSceneList.RemoveAt(popupDataSceneList.Count - 1);

                if (popupDataSceneList.Count == 0)
                {
                    popupDataList = null;
                    return;
                }

                (lastMainSceneId, lastPopupDataList) = popupDataSceneList[^1];
            }

            popupDataSceneList.RemoveAt(popupDataSceneList.Count - 1);
            popupDataList = lastPopupDataList;
        }

        void SuspendPopupFromSceneIdCore(MainSceneId mainSceneId)
        {
            popupDataSceneList.Add((mainSceneId, popupDataList?.ToList() ?? new List<IPopupData>()));
        }
    }
}