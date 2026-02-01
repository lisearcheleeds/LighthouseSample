using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneBase;
using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lighthouse.Core.Scene
{
    public class MainSceneGroup
    {
        public MainSceneKey CurrentMainSceneKey => currentScene != null ? currentScene.MainSceneId : null;

        public MainSceneKey[] GroupMainSceneIds { get; }

        public MainSceneBase CurrentScene => currentScene;
        public bool IsLoaded => loadedScenes != null;

        MainSceneBase currentScene;
        List<MainSceneBase> loadedScenes;

        Func<IDisposable> enqueueParentLifetimeScope;

        public MainSceneGroup(Func<IDisposable> enqueueParentLifetimeScope, params MainSceneKey[] groupMainSceneIds)
        {
            GroupMainSceneIds = groupMainSceneIds;
            this.enqueueParentLifetimeScope = enqueueParentLifetimeScope;
        }

        public ISceneCamera[] GetSceneCameraList()
        {
            if (currentScene != null)
            {
                return currentScene.GetSceneCameraList();
            }

            return null;
        }

        public async UniTask Enter(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken)
        {
            currentScene = loadedScenes.First(x => x.MainSceneId == transitionData.MainSceneKey);
            await currentScene.Enter(transitionData, transitionType, cancellationToken);
        }

        public async UniTask Leave(TransitionDataBase transitionData, TransitionType transitionType, CancellationToken cancellationToken)
        {
            await currentScene.Leave(transitionData, transitionType, cancellationToken);
        }

        public void ResetInAnimation(TransitionType transitionType)
        {
            if (currentScene != null)
            {
                currentScene.ResetInAnimation(transitionType);
            }
        }

        public async UniTask PlayInAnimation(TransitionDataBase transitionData, TransitionType transitionType)
        {
            await currentScene.PlayInAnimation(transitionType, true);
        }

        public async UniTask PlayOutAnimation(TransitionDataBase transitionData, TransitionType transitionType)
        {
            if (currentScene == null)
            {
                return;
            }

            await currentScene.PlayOutAnimation(transitionType, true);
        }

        public async UniTask SaveSceneState(CancellationToken cancelToken)
        {
            await currentScene.SaveSceneState(cancelToken);
        }

        public async UniTask Load(TransitionDataBase transitionData)
        {
            Debug.Assert(!loadedScenes?.Any() ?? true, "[MainSceneGroup] Duplicate load");

            loadedScenes = new List<MainSceneBase>();

            using (enqueueParentLifetimeScope.Invoke())
            {
                // The scene loading progress is not linear, and the loading itself is completed in 0.9f.
                // Call OnLoad before the scene's Active state to initialize the display.
                var loadOperations = GroupMainSceneIds
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

            currentScene = loadedScenes.First(x => x.MainSceneId == transitionData.MainSceneKey);
        }

        public async UniTask Unload()
        {
            await UniTask.WhenAll(loadedScenes.Select(s => s.OnUnload()));

            foreach (var loadedScene in loadedScenes)
            {
                await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(loadedScene.MainSceneId.Name);
            }

            loadedScenes.Clear();
            loadedScenes = null;
        }

        public void Suspend()
        {
            if (currentScene == null)
            {
                return;
            }

            currentScene.OnSuspend();
        }

        public void Resume()
        {
            if (currentScene == null)
            {
                return;
            }

            currentScene.OnResume();
        }

        MainSceneBase FindSceneBase(MainSceneKey mainSceneKey)
        {
            try
            {
                return UnityEngine.SceneManagement.SceneManager
                    .GetSceneByName(mainSceneKey.Name)
                    .GetRootGameObjects()
                    .Select(x => x.GetComponent<MainSceneBase>())
                    .First(x => x != null);
            }
            catch
            {
                Debug.LogError($"[MainSceneGroup] MainSceneBase NotFound\n" +
                               $"To add a scene, you need to add the scene to UnityEditor and place a GameObject that inherits MainSceneBase at the root of the added scene.");
                throw;
            }
        }
    }
}