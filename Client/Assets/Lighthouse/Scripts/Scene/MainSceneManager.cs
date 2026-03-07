using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneBase;
using Lighthouse.Scene.SceneCamera;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lighthouse.Scene
{
    public sealed class MainSceneManager : IMainSceneManager
    {
        Func<IDisposable> enqueueParentLifetimeScope;

        MainSceneBase currentScene;
        List<MainSceneBase> loadedScenes = new ();

        MainSceneId IMainSceneManager.CurrentMainSceneId => currentScene != null ? currentScene.MainSceneId : null;

        void IMainSceneManager.SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope)
        {
            this.enqueueParentLifetimeScope = enqueueParentLifetimeScope;
        }

        ISceneCamera[] IMainSceneManager.GetSceneCameraList(SceneTransitionDiff sceneTransitionDiff)
        {
            if (currentScene != null)
            {
                return currentScene.GetSceneCameraList();
            }

            return Array.Empty<ISceneCamera>();
        }

        void IMainSceneManager.InitializeCanvas(ISceneCameraManager sceneGroupManager, SceneTransitionDiff sceneTransitionDiff)
        {
            if (currentScene is ICanvasSceneBase canvasSceneBase)
            {
                canvasSceneBase.InitializeCanvas(sceneGroupManager.UICamera);
            }
        }

        async UniTask IMainSceneManager.Enter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken)
        {
            currentScene = loadedScenes.First(x => x.MainSceneId == transitionData.MainSceneId);
            await currentScene.Enter(transitionData, transitionType, true, cancellationToken);
        }

        async UniTask IMainSceneManager.Leave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken)
        {
            if (currentScene != null)
            {
                await currentScene.Leave(transitionData, transitionType, true, cancellationToken);
            }
        }

        void IMainSceneManager.ResetInAnimation(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            if (currentScene != null)
            {
                currentScene.ResetInAnimation(transitionType);
            }
        }

        async UniTask IMainSceneManager.PlayInAnimation(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            if (currentScene != null)
            {
                await currentScene.PlayInAnimation(transitionType, true);
            }
        }

        async UniTask IMainSceneManager.PlayOutAnimation(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            if (currentScene != null)
            {
                await currentScene.PlayOutAnimation(transitionType, true);
            }
        }

        async UniTask IMainSceneManager.SaveSceneState(CancellationToken cancelToken)
        {
            if (currentScene == null)
            {
                return;
            }

            await currentScene.SaveSceneState(cancelToken);
        }

        async UniTask IMainSceneManager.Load(SceneTransitionDiff sceneTransitionDiff)
        {
            if (sceneTransitionDiff.IsInnerGroupTransition)
            {
                return;
            }

            using (enqueueParentLifetimeScope.Invoke())
            {
                // The scene loading progress is not linear, and the loading itself is completed in 0.9f.
                // Call OnLoad before the scene's Active state to initialize the display.
                var loadOperations = sceneTransitionDiff.LoadMainSceneIds
                    .Select(sceneId => (sceneId: sceneId, ops: UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneId.Name, LoadSceneMode.Additive)))
                    .ToArray();

                await UniTask.WhenAll(loadOperations.Select(x => UniTask.WaitUntil(() => x.ops.progress >= 0.9f)));

                foreach (var loadOperation in loadOperations)
                {
                    loadOperation.ops.allowSceneActivation = true;
                }

                await UniTask.WhenAll(
                        loadOperations.Select(async x =>
                        {
                            await UniTask.WaitUntil(() =>
                            {
                                var s = UnityEngine.SceneManagement.SceneManager.GetSceneByName(x.sceneId.Name);
                                return s.IsValid() && s.isLoaded;
                            });

                            var scene = FindSceneBase(x.sceneId);
                            await scene.OnLoad();
                            await x.ops;
                            return scene;
                        }))
                    .ContinueWith(scenes =>
                    {
                        loadedScenes.AddRange(scenes);
                    });
            }

            currentScene = loadedScenes.First(x => x.MainSceneId == sceneTransitionDiff.NextMainSceneId);
        }

        async UniTask IMainSceneManager.Unload(SceneTransitionDiff sceneTransitionDiff)
        {
            if (sceneTransitionDiff.IsInnerGroupTransition)
            {
                return;
            }

            var unloadTargetMainScenes = loadedScenes.Where(loadedScene => sceneTransitionDiff.UnloadMainSceneIds.Contains(loadedScene.MainSceneId)).ToArray();
            await UniTask.WhenAll(unloadTargetMainScenes.Select(s => s.OnUnload()));

            await UniTask.WhenAll(unloadTargetMainScenes.Select(s => UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s.MainSceneId.Name).ToUniTask()));

            foreach (var unloadTargetMainScene in unloadTargetMainScenes)
            {
                loadedScenes.Remove(unloadTargetMainScene);
            }
        }

        void IMainSceneManager.Suspend()
        {
            if (currentScene == null)
            {
                return;
            }

            currentScene.OnSuspend();
        }

        void IMainSceneManager.Resume()
        {
            if (currentScene == null)
            {
                return;
            }

            currentScene.OnResume();
        }

        void IMainSceneManager.OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
            currentScene.OnSceneTransitionFinished(sceneTransitionDiff);
        }

        MainSceneBase FindSceneBase(MainSceneId mainSceneId)
        {
            try
            {
                return UnityEngine.SceneManagement.SceneManager
                    .GetSceneByName(mainSceneId.Name)
                    .GetRootGameObjects()
                    .Select(x => x.GetComponent<MainSceneBase>())
                    .First(x => x != null);
            }
            catch
            {
                Debug.LogError($"[SceneGroup] MainSceneBase NotFound\n" +
                               $"To add a scene, you need to add the scene to UnityEditor and place a GameObject that inherits MainSceneBase at the root of the added scene.");
                throw;
            }
        }
    }
}