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
    public sealed class SceneModuleManager : ISceneModuleManager
    {
        readonly List<SceneModuleBase> loadedSceneModules = new();

        ISceneCamera[] ISceneModuleManager.GetSceneCameraList(SceneModuleId[] requestSceneModuleIds)
        {
            return loadedSceneModules
                .Where(s => requestSceneModuleIds.Contains(s.SceneModuleId))
                .SelectMany(x => x.GetSceneCameraList() ?? Array.Empty<ISceneCamera>())
                .ToArray();
        }

        void ISceneModuleManager.ResetAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            var targetAnimations = loadedSceneModules
                .Where(x => sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.SceneModuleId) && !x.IsAlwaysInAnimation)
                .ToArray();

            foreach (var targetAnimation in targetAnimations)
            {
                targetAnimation.ResetInAnimation(transitionType);
            }
        }

        async UniTask ISceneModuleManager.PlayInAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            var targetAnimations = loadedSceneModules
                .Where(x =>
                {
                    if (sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.SceneModuleId))
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

        async UniTask ISceneModuleManager.PlayOutAnimation(TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff)
        {
            var targetAnimations = loadedSceneModules
                .Where(x =>
                {
                    if (!sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.SceneModuleId) || x.IsAlwaysOutAnimation)
                    {
                        if (x.VisibleStateType == VisibleStateType.Showing || x.VisibleStateType == VisibleStateType.Visible)
                        {
                            return true;
                        }
                    }

                    return false;
                })
                .Select(x => x.PlayOutAnimation(transitionType, !sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.SceneModuleId)))
                .ToArray();

            await UniTask.WhenAll(targetAnimations);
        }

        async UniTask ISceneModuleManager.Load(SceneTransitionDiff sceneTransitionDiff)
        {
            if (!sceneTransitionDiff.LoadSceneModuleIds.Any())
            {
                return;
            }

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

        async UniTask ISceneModuleManager.Unload(SceneTransitionDiff sceneTransitionDiff)
        {
            if (!sceneTransitionDiff.UnloadSceneModuleIds.Any())
            {
                return;
            }

            var unloadSceneModules = loadedSceneModules.Where(x => sceneTransitionDiff.UnloadSceneModuleIds.Contains(x.SceneModuleId)).ToArray();
            await UniTask.WhenAll(unloadSceneModules.Select(s => s.OnUnload()));

            await UniTask.WhenAll(unloadSceneModules.Select(s => UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s.SceneModuleId.Name).ToUniTask()));

            foreach (var unloadSceneModuleId in sceneTransitionDiff.UnloadSceneModuleIds)
            {
                var sceneModule = loadedSceneModules.First(x => x.SceneModuleId == unloadSceneModuleId);
                loadedSceneModules.Remove(sceneModule);
            }
        }

        async UniTask ISceneModuleManager.Enter(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken)
        {
            var target = loadedSceneModules
                .Where(x => sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(x.SceneModuleId))
                .Select(x => x.Enter(transitionData, transitionType, cancellationToken))
                .ToArray();

            await UniTask.WhenAll(target);
        }

        async UniTask ISceneModuleManager.Leave(TransitionDataBase transitionData, TransitionType transitionType, SceneTransitionDiff sceneTransitionDiff, CancellationToken cancellationToken)
        {
            if (sceneTransitionDiff?.CurrentSceneGroup == null)
            {
                return;
            }

            var target = loadedSceneModules
                .Where(x => sceneTransitionDiff.CurrentSceneGroup.SceneModuleIds.Contains(x.SceneModuleId))
                .Select(x => x.Leave(transitionData, transitionType, cancellationToken))
                .ToArray();

            await UniTask.WhenAll(target);
        }

        void ISceneModuleManager.InitializeCanvas(ISceneCameraManager sceneGroupManager, SceneTransitionDiff sceneTransitionDiff)
        {
            foreach (var sceneModule in loadedSceneModules)
            {
                if (sceneTransitionDiff.LoadSceneModuleIds.Contains(sceneModule.SceneModuleId) && sceneModule is ICanvasSceneBase canvasSceneModule)
                {
                    canvasSceneModule.InitializeCanvas(sceneGroupManager.UICamera);
                }
            }
        }

        void ISceneModuleManager.OnSceneTransitionFinished(SceneTransitionDiff sceneTransitionDiff)
        {
            foreach (var sceneModule in loadedSceneModules)
            {
                if (sceneTransitionDiff.NextSceneGroup.SceneModuleIds.Contains(sceneModule.SceneModuleId))
                {
                    sceneModule.OnSceneTransitionFinished(sceneTransitionDiff);
                }
            }
        }

        SceneModuleBase FindSceneBase(SceneModuleId sceneModuleId)
        {
            try
            {
                return UnityEngine.SceneManagement.SceneManager
                    .GetSceneByName(sceneModuleId.Name)
                    .GetRootGameObjects()
                    .Select(x => x.GetComponent<SceneModuleBase>())
                    .First(x => x != null);
            }
            catch
            {
                Debug.LogError($"[SceneModuleManager] SceneModule NotFound {sceneModuleId}");
                throw;
            }
        }
    }
}