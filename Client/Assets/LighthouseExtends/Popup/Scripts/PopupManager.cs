using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Input;
using Lighthouse.Scene;
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

        readonly Queue<Func<UniTask>> commandQueue = new();

        List<IPopupData> popupDataList;

        bool isProcessing;

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

        UniTask IPopupManager.EnqueuePopup(IPopupData popupData)
        {
            return EnqueueCommand(() =>
            {
                EnqueuePopupCore(popupData);
                return UniTask.CompletedTask;
            });
        }

        UniTask IPopupManager.OpenPopup()
        {
            return EnqueueCommand(() => OpenPopupCore(true));
        }

        UniTask IPopupManager.OpenPopup(IPopupData popupData)
        {
            return EnqueueCommand(async () =>
            {
                EnqueuePopupCore(popupData);
                await OpenPopupCore(true);
            });
        }

        UniTask IPopupManager.ClosePopup(IPopupData popupData)
        {
            return EnqueueCommand(() => ClosePopupCore(popupData));
        }

        UniTask IPopupManager.ClosePopup()
        {
            return EnqueueCommand(() => ClosePopupCore());
        }

        UniTask IPopupManager.ClearAllPopup()
        {
            return EnqueueCommand(() => ClearAllPopupCore());
        }

        UniTask IPopupManager.ClearCurrentAllPopup()
        {
            return EnqueueCommand(() => ClearCurrentAllPopupCore());
        }

        UniTask IPopupManager.ResumePopupFromSceneId(MainSceneId mainSceneId, bool isPlayInAnimation)
        {
            return EnqueueCommand(async () =>
            {
                ResumePopupFromSceneIdCore(mainSceneId);

                if (popupDataList?.Any() ?? false)
                {
                    await ResumeOpenPopupsCore(isPlayInAnimation);
                }
            });
        }

        UniTask IPopupManager.SuspendPopupFromSceneId(MainSceneId mainSceneId)
        {
            return EnqueueCommand(async () =>
            {
                SuspendPopupFromSceneIdCore(mainSceneId);
                await ClearCurrentAllPopupCore();
            });
        }

        UniTask EnqueueCommand(Func<UniTask> action)
        {
            var tcs = new UniTaskCompletionSource();

            commandQueue.Enqueue(async () =>
            {
                try
                {
                    await action();
                    tcs.TrySetResult();
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

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
                    await commandQueue.Dequeue()();
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

        async UniTask ResumeOpenPopupsCore(bool isPlayInAnimation)
        {
            try
            {
                for (var i = 0; i < popupDataList.Count; i++)
                {
                    var popupData = popupDataList[i];
                    var shouldPlayAnimation = isPlayInAnimation && i == popupDataList.Count - 1;

                    var prevPopupEntity = popupEntityList.LastOrDefault();
                    if (prevPopupEntity?.PopupData == popupData)
                    {
                        throw new InvalidOperationException($"Duplicate open");
                    }

                    var popupEntity = await popupEntityFactory.CreateAsync(popupData, CancellationToken.None);
                    if (shouldPlayAnimation)
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
                            prevPopupEntity.Popup.EndOutAnimation();
                        }

                        await prevPopupEntity.PopupPresenter.OnLeave();
                    }

                    await popupEntity.PopupPresenter.OnEnter(false);

                    if (shouldPlayAnimation)
                    {
                        await popupEntity.Popup.PlayInAnimation();
                    }

                    popupBackgroundInputBlocker.BlockPopupBackground(popupData.IsSystem);
                }
            }
            catch (Exception)
            {
                await ClearCurrentAllPopupCore();
                throw;
            }
        }

        async UniTask OpenPopupCore(bool isPlayInAnimation)
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

            var popupEntity = await popupEntityFactory.CreateAsync(popupData, CancellationToken.None);

            if (isPlayInAnimation)
            {
                popupEntity.Popup.ResetInAnimation();
            }
            else
            {
                popupEntity.Popup.EndInAnimation();
            }

            popupEntityList.Add(popupEntity);
            popupCanvasController.AddChild(popupEntity.Popup, popupData.IsSystem);

            try
            {
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

                if (isPlayInAnimation)
                {
                    await popupEntity.Popup.PlayInAnimation();
                }

                popupBackgroundInputBlocker.BlockPopupBackground(popupData.IsSystem);
            }
            catch
            {
                popupEntityList.Remove(popupEntity);
                popupEntity.Popup.Dispose();
                if (prevPopupEntity != null && !popupData.IsOverlayOpen)
                {
                    prevPopupEntity.Popup.EndInAnimation();
                }

                throw;
            }
        }

        async UniTask ClosePopupCore(IPopupData popupData)
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

            try
            {
                await target.Popup.PlayOutAnimation();
                await target.PopupPresenter.OnLeave();
            }
            finally
            {
                target.Popup.Dispose();
            }

            await UniTask.DelayFrame(1);

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

            try
            {
                await prevPopup.PopupPresenter.OnEnter(true);

                if (!popupData.IsOverlayOpen)
                {
                    await prevPopup.Popup.PlayInAnimation();
                }
            }
            catch
            {
                prevPopup.Popup.EndInAnimation();
                throw;
            }
            finally
            {
                popupBackgroundInputBlocker.BlockPopupBackground(prevPopup.PopupData.IsSystem);
            }
        }

        async UniTask ClosePopupCore()
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

            try
            {
                await currentPopup.Popup.PlayOutAnimation();
                await currentPopup.PopupPresenter.OnLeave();
            }
            finally
            {
                currentPopup.Popup.Dispose();
            }

            await UniTask.DelayFrame(1);

            var prevPopup = popupEntityList.LastOrDefault();
            if (prevPopup == null)
            {
                popupBackgroundInputBlocker.UnBlock();
                return;
            }

            try
            {
                await prevPopup.PopupPresenter.OnEnter(true);

                if (!lastPopupData.IsOverlayOpen)
                {
                    await prevPopup.Popup.PlayInAnimation();
                }
            }
            catch
            {
                prevPopup.Popup.EndInAnimation();
                throw;
            }
            finally
            {
                popupBackgroundInputBlocker.BlockPopupBackground(prevPopup.PopupData.IsSystem);
            }
        }

        UniTask ClearAllPopupCore()
        {
            popupDataSceneList.Clear();
            return ClearCurrentAllPopupCore();
        }

        async UniTask ClearCurrentAllPopupCore()
        {
            popupDataList?.Clear();

            var lastTarget = popupEntityList.LastOrDefault();
            while (0 < popupEntityList.Count)
            {
                var target = popupEntityList[^1];
                var isLast = ReferenceEquals(target, lastTarget);
                popupEntityList.RemoveAt(popupEntityList.Count - 1);

                try
                {
                    if (isLast || target.PopupData.IsOverlayOpen)
                    {
                        await target.Popup.PlayOutAnimation();
                    }

                    await target.PopupPresenter.OnLeave();
                }
                finally
                {
                    target.Popup.Dispose();
                }
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
                popupDataSceneList.RemoveAt(popupDataSceneList.Count - 1);

                if (!popupDataSceneList.Any())
                {
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