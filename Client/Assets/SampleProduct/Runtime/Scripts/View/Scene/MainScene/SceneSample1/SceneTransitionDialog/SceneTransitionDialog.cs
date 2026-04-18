using Cysharp.Threading.Tasks;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
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

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => sceneTransitionView.TryClickCloseButton());
        }

        public void Setup(SceneTransitionData data)
        {
            presenter.Bind(sceneTransitionView, data);
        }
    }
}
