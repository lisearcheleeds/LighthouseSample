using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene.SceneBase;
using Lighthouse.Core.Scene.SceneCamera;
using UnityEngine;

namespace Lighthouse.Core.Scene.SceneTransitionStep
{
    public sealed class ResolveCameraStep : ISceneTransitionStep
    {
        UniTask ISceneTransitionStep.Run(
            TransitionDataBase transitionData,
            TransitionType transitionType,
            MainSceneKey beforeMainSceneKey,
            MainSceneGroup beforeMainSceneGroup,
            MainSceneGroup afterMainSceneGroup,
            ISceneCameraManager sceneCameraManager,
            ICommonSceneManager commonSceneManager,
            CancellationToken cancelToken)
        {
            sceneCameraManager.UpdateCameraStack(afterMainSceneGroup, commonSceneManager, transitionData.RequireCommonSceneIds);

            if (!afterMainSceneGroup.IsLoaded)
            {
                Debug.LogError("[ResolveCameraStep] Can not resolve the camera before loading.");
            }

            if (afterMainSceneGroup.CurrentScene is ICanvasSceneBase mainSceneBase)
            {
                mainSceneBase.InitializeCanvas(sceneCameraManager.UICamera);
            }

            commonSceneManager.InitializeCanvas(sceneCameraManager.UICamera, transitionData.RequireCommonSceneIds);

            return UniTask.CompletedTask;
        }
    }
}