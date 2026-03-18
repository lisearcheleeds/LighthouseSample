using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Input;
using Lighthouse.Scene;
using UnityEngine;
using VContainer;

namespace LighthouseExtends.Popup
{
    public sealed class PopupManager : IPopupManager
    {
        readonly IPopupCanvasController popupCanvasController;
        readonly IPopupEntityFactory popupEntityFactory;
        readonly IPopupBackgroundInputBlocker popupBackgroundInputBlocker;
        readonly IInputBlocker inputBlocker;

        readonly List<(MainSceneId, List<IPopupData>)> popupDataSceneList = new();
        readonly List<PopupEntity> popupEntityList = new();

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
        public PopupManager(
            IPopupCanvasController popupCanvasController,
            IPopupEntityFactory popupEntityFactory,
            IPopupBackgroundInputBlocker popupBackgroundInputBlocker,
            IInputBlocker inputBlocker)
        {
            this.popupCanvasController = popupCanvasController;
            this.popupEntityFactory = popupEntityFactory;
            this.popupBackgroundInputBlocker = popupBackgroundInputBlocker;
            this.inputBlocker = inputBlocker;
        }

        void IPopupManager.Setup()
        {
            popupBackgroundInputBlocker.Setup();
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
            return EnqueueCommand(() => OpenPopupCore(true, token), token);
        }

        UniTask IPopupManager.OpenPopup(IPopupData popupData, CancellationToken token)
        {
            return EnqueueCommand(async () =>
            {
                EnqueuePopupCore(popupData);
                await OpenPopupCore(true, token);
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
                    await ResumeOpenPopupsCore(token);
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
            try
            {
                inputBlocker.Block<PopupManager>();

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
            }
            finally
            {
                inputBlocker.UnBlock<PopupManager>();
                isProcessing = false;
            }
        }

        void EnqueuePopupCore(IPopupData popupData)
        {
            if (popupDataList == null)
            {
                popupDataList = new List<IPopupData>();
            }

            popupDataList.Add(popupData);
        }

        async UniTask ResumeOpenPopupsCore(CancellationToken token)
        {
            for (var i = 0; i < popupDataList.Count; i++)
            {
                var popupData = popupDataList[i];

                var prevPopupEntity = popupEntityList.LastOrDefault();
                if (prevPopupEntity?.PopupData == popupData)
                {
                    throw new InvalidOperationException($"Duplicate open");
                }

                var popupEntity = await popupEntityFactory.CreateAsync(popupData, token);
                popupEntity.Popup.EndInAnimation();

                popupEntityList.Add(popupEntity);
                popupCanvasController.AddChild(popupEntity.Popup, popupData.IsSystem);

                await popupEntity.Popup.OnInitialize();

                if (prevPopupEntity != null)
                {
                    if (!popupData.IsOverlayOpen)
                    {
                        prevPopupEntity.Popup.EndOutAnimation();
                    }

                    await prevPopupEntity.PopupPresenter.OnLeave();
                }

                await popupEntity.PopupPresenter.OnEnter(false);
            }

            popupBackgroundInputBlocker.BlockPopupBackground(popupDataList[^1].IsSystem);
        }

        async UniTask OpenPopupCore(bool playInAnimation, CancellationToken token)
        {
            var popupData = popupDataList?.LastOrDefault();
            if (popupData == null)
            {
                throw new InvalidOperationException($"Empty popup data");
            }

            var prevPopupEntity = popupEntityList.LastOrDefault();
            if (prevPopupEntity?.PopupData == popupData)
            {
                throw new InvalidOperationException($"Duplicate open");
            }

            var popupEntity = await popupEntityFactory.CreateAsync(popupData, token);

            if (playInAnimation)
            {
                popupEntity.Popup.ResetInAnimation();
            }
            else
            {
                popupEntity.Popup.EndInAnimation();
            }

            popupEntityList.Add(popupEntity);
            popupCanvasController.AddChild(popupEntity.Popup, popupData.IsSystem);

            await popupEntity.Popup.OnInitialize();

            if (prevPopupEntity != null)
            {
                if (!popupData.IsOverlayOpen)
                {
                    await prevPopupEntity.Popup.PlayOutAnimation();
                }

                await prevPopupEntity.PopupPresenter.OnLeave();
            }

            await popupEntity.PopupPresenter.OnEnter(false);

            if (playInAnimation)
            {
                await popupEntity.Popup.PlayInAnimation();
            }

            popupBackgroundInputBlocker.BlockPopupBackground(popupData.IsSystem);
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

            if (!(popupEntityList?.Any() ?? false))
            {
                return;
            }

            // If there is a popup view, remove it too.
            var target = popupEntityList.FirstOrDefault(x => ReferenceEquals(x.PopupData, popupData));
            if (target == null)
            {
                return;
            }

            var isLast = ReferenceEquals(target, popupEntityList[^1]);
            popupEntityList.Remove(target);

            await target.Popup.PlayOutAnimation();
            await target.PopupPresenter.OnLeave();
            target.Popup.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: token);

            if (!isLast)
            {
                return;
            }

            var prevPopup = popupEntityList.LastOrDefault();
            if (prevPopup == null)
            {
                popupBackgroundInputBlocker.UnBlock();
                return;
            }

            await prevPopup.PopupPresenter.OnEnter(true);

            if (!popupData.IsOverlayOpen)
            {
                await prevPopup.Popup.PlayInAnimation();
            }

            popupBackgroundInputBlocker.BlockPopupBackground(prevPopup.PopupData.IsSystem);
        }

        async UniTask ClosePopupCore(CancellationToken token)
        {
            var lastPopupData = popupDataList?.LastOrDefault();
            if (lastPopupData == null || !popupDataList.Remove(lastPopupData))
            {
                return;
            }

            var currentPopup = popupEntityList.LastOrDefault();
            if (currentPopup == null)
            {
                return;
            }

            popupEntityList.Remove(currentPopup);

            await currentPopup.Popup.PlayOutAnimation();
            await currentPopup.PopupPresenter.OnLeave();
            currentPopup.Popup.Dispose();

            await UniTask.DelayFrame(1, cancellationToken: token);

            var prevPopup = popupEntityList.LastOrDefault();
            if (prevPopup == null)
            {
                popupBackgroundInputBlocker.UnBlock();
                return;
            }

            await prevPopup.PopupPresenter.OnEnter(true);

            if (!lastPopupData.IsOverlayOpen)
            {
                await prevPopup.Popup.PlayInAnimation();
            }

            popupBackgroundInputBlocker.BlockPopupBackground(prevPopup.PopupData.IsSystem);
        }

        UniTask ClearAllPopupCore(CancellationToken token)
        {
            popupDataSceneList.Clear();
            return ClearCurrentAllPopupCore(token);
        }

        async UniTask ClearCurrentAllPopupCore(CancellationToken token)
        {
            popupDataList?.Clear();

            var lastTarget = popupEntityList.LastOrDefault();
            while (0 < popupEntityList.Count)
            {
                var target = popupEntityList[^1];
                var isLast = ReferenceEquals(target, lastTarget);
                popupEntityList.RemoveAt(popupEntityList.Count - 1);

                if (isLast || target.PopupData.IsOverlayOpen)
                {
                    await target.Popup.PlayOutAnimation();
                }

                await target.PopupPresenter.OnLeave();
                target.Popup.Dispose();
            }

            popupBackgroundInputBlocker.UnBlock();
            popupEntityList.Clear();
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
                if (popupDataSceneList.Count == 0)
                {
                    return;
                }

                popupDataSceneList.RemoveAt(popupDataSceneList.Count - 1);
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