using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneBase;
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

        public async UniTask ResetAnimation(TransitionType transitionType)
        {
            if (currentScene != null)
            {
                await currentScene.ResetAnimation(transitionType);
            }
        }

        public async UniTask InAnimation(TransitionDataBase transitionData, TransitionType transitionType)
        {
            await currentScene.InAnimation(transitionType);
        }

        public async UniTask OutAnimation(TransitionDataBase transitionData, TransitionType transitionType)
        {
            if (currentScene == null)
            {
                return;
            }

            await currentScene.OutAnimation(transitionType);
        }

        public async UniTask SaveSceneState(CancellationToken cancelToken)
        {
            await currentScene.SaveSceneState(cancelToken);
        }

        public async UniTask Load()
        {
            Debug.Assert(!loadedScenes?.Any() ?? true, "[MainSceneGroup] Duplicate load");

            using (enqueueParentLifetimeScope.Invoke())
            {
                foreach (var sceneId in GroupMainSceneIds)
                {
                    await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneId.Name, LoadSceneMode.Additive);
                }
            }

            await UniTask.DelayFrame(1);

            loadedScenes = new List<MainSceneBase>();
            loadedScenes.AddRange(GroupMainSceneIds.Select(FindSceneBase));

            await UniTask.WhenAll(loadedScenes.Select(s => s.OnLoad()));
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