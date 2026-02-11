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
    public sealed class ModuleSceneManager : IModuleSceneManager
    {
        Func<IDisposable> enqueueParentLifetimeScope;

        readonly List<ModuleSceneBase> loadedSceneModules = new();

        void IModuleSceneManager.SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope)
        {
            this.enqueueParentLifetimeScope = enqueueParentLifetimeScope;
        }

        ISceneCamera[] IModuleSceneManager.GetSceneCameraList(ModuleSceneId[] requestSceneModuleIds)
        {
            return loadedSceneModules
                .Where(s => requestSceneModuleIds.Contains(s.ModuleSceneId))
                .SelectMany(x => x.GetSceneCameraList() ?? Array.Empty<ISceneCamera>())
                .ToArray();
        }

        void IModuleSceneManager.ResetAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            var targetAnimations = loadedSceneModules
                .Where(x => sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.ModuleSceneId))
                .ToArray();

            foreach (var targetAnimation in targetAnimations)
            {
                targetAnimation.ResetInAnimation(transitionType);
            }
        }

        async UniTask IModuleSceneManager.PlayInAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            var targetAnimations = loadedSceneModules
                .Where(x =>
                {
                    if (sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.ModuleSceneId))
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

        async UniTask IModuleSceneManager.PlayOutAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            var targetAnimations = loadedSceneModules
                .Where(x =>
                {
                    if (!sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.ModuleSceneId) || x.IsAlwaysOutAnimation)
                    {
                        if (x.VisibleStateType == VisibleStateType.Showing || x.VisibleStateType == VisibleStateType.Visible)
                        {
                            return true;
                        }
                    }

                    return false;
                })
                .Select(x => x.PlayOutAnimation(transitionType, !sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.ModuleSceneId)))
                .ToArray();

            await UniTask.WhenAll(targetAnimations);
        }

        async UniTask IModuleSceneManager.Load(SceneTransitionDiff sceneTransitionDiff)
        {
            if (!sceneTransitionDiff.LoadSceneModuleIds.Any())
            {
                return;
            }

            using (enqueueParentLifetimeScope.Invoke())
            {
                // The scene loading progress is not linear, and the loading itself is completed in 0.9f.
                // Call OnLoad before the scene's Active state to initialize the display.
                var loadOperations = sceneTransitionDiff.LoadSceneModuleIds
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
                        loadedSceneModules.AddRange(scenes);
                    });
            }
        }

        async UniTask IModuleSceneManager.Unload(SceneTransitionDiff sceneTransitionDiff)
        {
            if (!sceneTransitionDiff.UnloadSceneModuleIds.Any())
            {
                return;
            }

            var unloadSceneModules = loadedSceneModules.Where(x => sceneTransitionDiff.UnloadSceneModuleIds.Contains(x.ModuleSceneId)).ToArray();
            await UniTask.WhenAll(unloadSceneModules.Select(s => s.OnUnload()));

            await UniTask.WhenAll(unloadSceneModules.Select(s => UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s.ModuleSceneId.Name).ToUniTask()));

            foreach (var unloadSceneModuleId in sceneTransitionDiff.UnloadSceneModuleIds)
            {
                var sceneModule = loadedSceneModules.First(x => x.ModuleSceneId == unloadSceneModuleId);
                loadedSceneModules.Remove(sceneModule);
            }
        }

        async UniTask IModuleSceneManager.Enter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken)
        {
            var target = loadedSceneModules
                .Where(x => sceneTransitionDiff.ActivateSceneModuleIds.Contains(x.ModuleSceneId))
                .Select(x => x.Enter(transitionData, transitionType, cancellationToken))
                .ToArray();

            await UniTask.WhenAll(target);
        }

        async UniTask IModuleSceneManager.Leave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken)
        {
            var target = loadedSceneModules
                .Where(x => sceneTransitionDiff.DeactivateSceneModuleIds.Contains(x.ModuleSceneId))
                .Select(x => x.Leave(transitionData, transitionType, cancellationToken))
                .ToArray();

            await UniTask.WhenAll(target);
        }

        void IModuleSceneManager.InitializeCanvas(ISceneCameraManager sceneGroupManager, SceneTransitionDiff sceneTransitionDiff)
        {
            foreach (var sceneModule in loadedSceneModules)
            {
                if (sceneTransitionDiff.LoadSceneModuleIds.Contains(sceneModule.ModuleSceneId) && sceneModule is ICanvasSceneBase canvasSceneModule)
                {
                    canvasSceneModule.InitializeCanvas(sceneGroupManager.UICamera);
                }
            }
        }

        void IModuleSceneManager.OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
            foreach (var sceneModule in loadedSceneModules)
            {
                if (sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(sceneModule.ModuleSceneId))
                {
                    sceneModule.OnSceneTransitionFinished(sceneTransitionDiff);
                }
            }
        }

        ModuleSceneBase FindSceneBase(ModuleSceneId moduleSceneId)
        {
            try
            {
                return UnityEngine.SceneManagement.SceneManager
                    .GetSceneByName(moduleSceneId.Name)
                    .GetRootGameObjects()
                    .Select(x => x.GetComponent<ModuleSceneBase>())
                    .First(x => x != null);
            }
            catch
            {
                Debug.LogError($"[ModuleSceneManager] SceneModule NotFound {moduleSceneId}");
                throw;
            }
        }
    }
}