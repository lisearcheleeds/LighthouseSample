using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.OverlayElement
{
    public sealed class OverlayElementOverlay : ProductScreenStackBase, IScreenStackSetup<OverlayElementData>
    {
        [SerializeField] OverlayElementView overlayElementView;

        OverlayElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new OverlayElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => overlayElementView.TryClickCloseButton());
        }

        public void Setup(OverlayElementData screenStackData)
        {
            presenter.Bind(overlayElementView, screenStackData);
        }
    }
}
