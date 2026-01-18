using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class CleanupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            MainSceneKey beforeMainSceneKey,
            MainSceneGroup beforeMainSceneGroup,
            MainSceneGroup afterMainSceneGroup,
            ISceneCameraManager sceneCameraManager,
            ICommonSceneManager commonSceneManager,
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