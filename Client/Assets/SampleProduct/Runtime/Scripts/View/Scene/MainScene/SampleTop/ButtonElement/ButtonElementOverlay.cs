using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.ButtonElement
{
    public sealed class ButtonElementOverlay : ProductScreenStackBase, IScreenStackSetup<ButtonElementData>
    {
        [SerializeField] ButtonElementView buttonElementView;

        ButtonElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new ButtonElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => buttonElementView.TryClickCloseButton());
        }

        public void Setup(ButtonElementData screenStackData)
        {
            presenter.Bind(buttonElementView, screenStackData);
        }
    }
}
