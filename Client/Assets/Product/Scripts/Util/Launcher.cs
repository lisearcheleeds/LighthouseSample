using Cysharp.Threading.Tasks;
using Lighthouse.Core.Scene;
using Product.View.Home;
using Product.View.Splash;
using Product.View.Title;
using UnityEngine;
using VContainer;

namespace Product.Util
{
    public sealed class Launcher : ILauncher
    {
        static readonly string LauncherSceneName = "Launcher";

        SceneManager sceneManager;
        ISceneGroupController sceneGroupController;

        [Inject]
        public Launcher(SceneManager sceneManager, ISceneGroupController sceneGroupController)
        {
            this.sceneManager = sceneManager;
            this.sceneGroupController = sceneGroupController;
        }

        void ILauncher.Reboot()
        {
            RebootProcess().Forget();

            async UniTask RebootProcess()
            {
                await sceneGroupController.PreReboot();

                await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LauncherSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                await LaunchProcess();

                TransitionNextScene();
            }
        }

        async UniTask ILauncher.Launch()
        {
            await FirstLaunchProcess();
            await LaunchProcess();
            TransitionNextScene();
        }

        void TransitionNextScene()
        {
            sceneManager.TransitionScene(
                new SplashScene.SplashTransitionData(),
                onComplete: _ =>
                {
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(LauncherSceneName);
                });
        }

        UniTask FirstLaunchProcess()
        {
            return UniTask.CompletedTask;
        }

        UniTask LaunchProcess()
        {
            return UniTask.CompletedTask;
        }
    }
}