using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighthouse.Scene.SceneTransitionStep
{
    public sealed class CleanupStep : ISceneTransitionStep
    {
        async UniTask ISceneTransitionStep.Run(
            SceneTransitionContext context,
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