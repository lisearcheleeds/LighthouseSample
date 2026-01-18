using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneBase;
using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine.SceneManagement;

namespace Lighthouse.Core.Scene
{
    public sealed class CommonSceneManager : ICommonSceneManager
    {
        readonly List<CommonSceneBase> loadedCommonScenes = new();
        readonly List<ICanvasSceneBase> loadedCanvasSceneBases = new();

        public ISceneCamera[] GetSceneCameraList(CommonSceneKey[] requestCommonSceneIds)
        {
            return loadedCommonScenes
                .Where(s => requestCommonSceneIds.Contains(s.CommonSceneId))
                .SelectMany(x => x.GetSceneCameraList() ?? Array.Empty<ISceneCamera>())
                .ToArray();
        }

        public async UniTask PlayResetAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType)
        {
            var targetAnimations = loadedCommonScenes
                .Where(x => requestCommonSceneIds.Contains(x.CommonSceneId) && !x.IsAlwaysInAnimation)
                .Select(x => x.PlayResetAnimation(transitionType))
                .ToArray();

            await UniTask.WhenAll(targetAnimations);
        }

        public async UniTask PlayInAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType)
        {
            var targetAnimations = loadedCommonScenes
                .Where(x =>
                {
                    if (requestCommonSceneIds.Contains(x.CommonSceneId))
                    {
                        if (x.VisibleStateType == VisibleStateType.Hidden || x.VisibleStateType == VisibleStateType.Hiding || x.IsAlwaysInAnimation)
                        {
                            return true;
                        }
                    }

                    return false;
                })
                .Select(x => x.PlayInAnimation(transitionType, (x.VisibleStateType == VisibleStateType.Hidden || x.VisibleStateType == VisibleStateType.Hiding)))
                .ToArray();

            await UniTask.WhenAll(targetAnimations);
        }

        public async UniTask PlayOutAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType)
        {
            var targetAnimations = loadedCommonScenes
                .Where(x =>
                {
                    if (!requestCommonSceneIds.Contains(x.CommonSceneId) || x.IsAlwaysOutAnimation)
                    {
                        if (x.VisibleStateType == VisibleStateType.Showing || x.VisibleStateType == VisibleStateType.Visible)
                        {
                            return true;
                        }
                    }

                    return false;
                })
                .Select(x => x.PlayOutAnimation(transitionType, !requestCommonSceneIds.Contains(x.CommonSceneId)))
                .ToArray();

            await UniTask.WhenAll(targetAnimations);
        }

        public async UniTask LoadCommonScenes(CommonSceneKey[] requestCommonSceneIds)
        {
            var currentCommonSceneIds = loadedCommonScenes.Select(x => x.CommonSceneId).ToArray();
            var needLoadSceneIds = requestCommonSceneIds.Except(currentCommonSceneIds).ToArray();

            if (!needLoadSceneIds.Any())
            {
                return;
            }

            await UniTask.WhenAll(needLoadSceneIds.Select(x => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(x.Name, LoadSceneMode.Additive).ToUniTask()));

            await UniTask.DelayFrame(1);

            var loadedScenes = needLoadSceneIds
                .Select(needLoadSceneId => UnityEngine.SceneManagement.SceneManager
                    .GetSceneByName(needLoadSceneId.Name)
                    .GetRootGameObjects()
                    .Select(x => x.GetComponent<CommonSceneBase>())
                    .First(x => x != null))
                .ToArray();

            loadedCommonScenes.AddRange(loadedScenes);
            loadedCanvasSceneBases.AddRange(loadedScenes.OfType<ICanvasSceneBase>());

            await UniTask.WhenAll(loadedScenes.Select(s => s.OnLoad()));
        }

        public async UniTask UnloadUnusedCommonScenes(CommonSceneKey[] requireCommonSceneIds)
        {
            var unloadCommonSceneIds = loadedCommonScenes.Select(x => x.CommonSceneId).Except(requireCommonSceneIds).ToArray();

            if (!unloadCommonSceneIds.Any())
            {
                return;
            }

            var unloadCommonScenes = loadedCommonScenes.Where(x => unloadCommonSceneIds.Contains(x.CommonSceneId)).ToArray();
            await UniTask.WhenAll(unloadCommonScenes.Select(s => s.OnUnload()));

            await UniTask.WhenAll(unloadCommonScenes.Select(s => UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s.CommonSceneId.Name).ToUniTask()));

            foreach (var unloadCommonSceneId in unloadCommonSceneIds)
            {
                var commonScene = loadedCommonScenes.First(x => x.CommonSceneId == unloadCommonSceneId);
                loadedCommonScenes.Remove(commonScene);

                if (commonScene is ICanvasSceneBase commonCanvasSceneBase)
                {
                    loadedCanvasSceneBases.Remove(commonCanvasSceneBase);
                }
            }
        }

        public async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken)
        {
            var target = loadedCommonScenes
                .Where(x => transitionData.RequireCommonSceneIds.Contains(x.CommonSceneId))
                .Select(x => x.Enter(transitionData, transitionType, cancellationToken))
                .ToArray();

            await UniTask.WhenAll(target);
        }

        public async UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken)
        {
            var target = loadedCommonScenes
                .Where(x => !transitionData.RequireCommonSceneIds.Contains(x.CommonSceneId))
                .Select(x => x.Leave(transitionData, transitionType, cancellationToken))
                .ToArray();

            await UniTask.WhenAll(target);
        }

        public void InitializeCanvas(ISceneCamera sceneCamera, CommonSceneKey[] requestCommonSceneIds)
        {
            foreach (var commonScene in loadedCommonScenes)
            {
                if (requestCommonSceneIds.Contains(commonScene.CommonSceneId) && commonScene is ICanvasSceneBase canvasCommonScene)
                {
                    canvasCommonScene.InitializeCanvas(sceneCamera);
                }
            }
        }

        public void OnSceneTransitionFinished(CommonSceneKey[] requestCommonSceneIds)
        {
            foreach (var commonScene in loadedCommonScenes)
            {
                if (requestCommonSceneIds.Contains(commonScene.CommonSceneId))
                {
                    commonScene.OnSceneTransitionFinished();
                }
            }
        }

        public T GetCommonScene<T>() where T: CommonSceneBase
        {
            return loadedCommonScenes.OfType<T>().FirstOrDefault();
        }
    }
}