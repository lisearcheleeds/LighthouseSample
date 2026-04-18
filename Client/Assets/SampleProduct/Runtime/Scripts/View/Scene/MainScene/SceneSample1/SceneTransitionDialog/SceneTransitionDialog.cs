using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionDialog
{
    public sealed class SceneTransitionDialog : StandardDialogBase, IScreenStackSetup<SceneTransitionData>
    {
        [FormerlySerializedAs("sceneTransitionPopupView")] [SerializeField] SceneTransitionView sceneTransitionView;

        SceneTransitionPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new SceneTransitionPresenter();
            objectResolver.Inject(presenter);
        }

        public void Setup(SceneTransitionData data)
        {
            presenter.Bind(sceneTransitionView, data);
        }
    }
}
