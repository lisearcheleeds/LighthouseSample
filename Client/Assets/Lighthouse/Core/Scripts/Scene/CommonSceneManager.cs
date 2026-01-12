using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lighthouse.Core.Scene
{
    public class CommonSceneManager
    {
        readonly List<CommonSceneBase> loadedCommonScenes = new();
        readonly List<ICanvasSceneBase> loadedCanvasSceneBases = new();

        public IReadOnlyList<ICanvasSceneBase> LoadedCanvasSceneBases => loadedCanvasSceneBases;

        public ISceneCamera[] GetSceneCameraList(CommonSceneKey[] requestCommonSceneIds)
        {
            return loadedCommonScenes
                .Where(s => requestCommonSceneIds.Contains(s.CommonSceneId))
                .SelectMany(x => x.GetSceneCameraList())
                .ToArray();
        }

        public async UniTask ResetAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType)
        {
            await UniTask.WhenAll(loadedCommonScenes
                .Where(s => requestCommonSceneIds.Contains(s.CommonSceneId))
                .Select(x => x.ResetAnimation(transitionType)));
        }

        public async UniTask InAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType)
        {
            await UniTask.WhenAll(
                loadedCommonScenes
                    .Where(x => (x.VisibleStateType == VisibleStateType.Hidden || x.VisibleStateType == VisibleStateType.Hiding)
                                && requestCommonSceneIds.Contains(x.CommonSceneId))
                    .Select(x => x.InAnimation(transitionType)));
        }

        public async UniTask OutAnimation(CommonSceneKey[] requestCommonSceneIds, TransitionType transitionType)
        {
            await UniTask.WhenAll(
                loadedCommonScenes
                    .Where(x => (x.VisibleStateType == VisibleStateType.Showing || x.VisibleStateType == VisibleStateType.Visible)
                                && !requestCommonSceneIds.Contains(x.CommonSceneId))
                    .Select(x => x.OutAnimation(transitionType)));
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
            await UniTask.WhenAll(loadedCommonScenes
                .Where(x => transitionData.RequireCommonSceneIds.Contains(x.CommonSceneId))
                .Select(x => x.Enter(transitionData, transitionType, cancellationToken)));
        }

        public async UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken)
        {
            await UniTask.WhenAll(loadedCommonScenes
                .Where(x => !transitionData.RequireCommonSceneIds.Contains(x.CommonSceneId))
                .Select(x => x.Leave(transitionData, transitionType, cancellationToken)));
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

        public virtual void OnSceneTransitionFinished(CommonSceneKey[] requestCommonSceneIds)
        {
            foreach (var commonScene in loadedCommonScenes)
            {
                if (requestCommonSceneIds.Contains(commonScene.CommonSceneId))
                {
                    commonScene.OnSceneTransitionFinished();
                }
            }
        }
    }
}