using Cysharp.Threading.Tasks;
using Lighthouse.Scene;
using SampleProduct.View.Scene.MainScene.Splash;
using UnityEngine;
using VContainer;

namespace SampleProduct.Core
{
    public sealed class Launcher : ILauncher
    {
        static readonly string LauncherSceneName = "Launcher";

        ISceneManager sceneManager;

        [Inject]
        public Launcher(ISceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        void ILauncher.Reboot()
        {
            RebootProcess().Forget();

            async UniTask RebootProcess()
            {
                await sceneManager.PreReboot();

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

        UniTask FirstLaunchProcess()
        {
            return UniTask.CompletedTask;
        }

        UniTask LaunchProcess()
        {
            return UniTask.CompletedTask;
        }

        void TransitionNextScene()
        {
            sceneManager.TransitionScene(
                new SplashScene.SplashTransitionData(),
                onComplete: _ =>
                {
                    if (!string.IsNullOrEmpty(UnityEngine.SceneManagement.SceneManager.GetSceneByName(LauncherSceneName).name))
                    {
                        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(LauncherSceneName);
                    }
                });
        }
    }
}