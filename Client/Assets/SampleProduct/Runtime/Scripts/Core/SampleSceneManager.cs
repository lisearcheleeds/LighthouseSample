using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using UnityEngine;
using VContainer;

namespace SampleProduct.Core
{
    /// <summary>
    /// Wraps Lighthouse's SceneManager and catches unhandled transition exceptions.
    /// When an unexpected exception occurs, it logs the error and reboots the application
    /// rather than attempting partial recovery, which could leave the app in an inconsistent state.
    /// </summary>
    public sealed class SampleSceneManager : ISampleSceneManager
    {
        readonly ISceneManager sceneManager;
        readonly ILauncher launcher;

        public bool IsTransition => sceneManager.IsTransition;

        [Inject]
        public SampleSceneManager(ISceneManager sceneManager, ILauncher launcher)
        {
            this.sceneManager = sceneManager;
            this.launcher = launcher;
        }

        async UniTask ISampleSceneManager.TransitionScene(
            TransitionDataBase nextTransitionData,
            TransitionType transitionType,
            MainSceneId backMainSceneId)
        {
            try
            {
                await sceneManager.TransitionScene(nextTransitionData, transitionType, backMainSceneId);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SampleSceneManager] Unhandled exception during transition. Rebooting.\n{e}");

                // NOTE: The sample does not reboot.
                // In a real project, it is recommended to reboot after displaying a dialog box and reporting errors.
                // launcher.Reboot();
            }
        }

        async UniTask ISampleSceneManager.BackScene(TransitionType transitionType)
        {
            try
            {
                await sceneManager.BackScene(transitionType);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SampleSceneManager] Unhandled exception during back transition. Rebooting.\n{e}");
                launcher.Reboot();
            }
        }

        UniTask ISampleSceneManager.PreReboot() => sceneManager.PreReboot();
    }
}
