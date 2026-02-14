using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class CleanupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            SceneTransitionDiff sceneTransitionDiff,
            IMainSceneManager mainSceneManager,
            IModuleSceneManager moduleSceneManager,
            ISceneCameraManager sceneCameraManager,
            CancellationToken cancelToken)
        {
            // Before GC
            GC.Collect();

            await Resources.UnloadUnusedAssets();

            // After GC
            GC.Collect();
        }
    }
}