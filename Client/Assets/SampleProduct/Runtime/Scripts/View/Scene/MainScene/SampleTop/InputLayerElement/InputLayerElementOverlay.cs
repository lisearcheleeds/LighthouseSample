using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.InputLayerElement
{
    public sealed class InputLayerElementOverlay : ProductScreenStackBase, IScreenStackSetup<InputLayerElementData>
    {
        [SerializeField] InputLayerElementView inputLayerElementView;

        InputLayerElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new InputLayerElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => inputLayerElementView.TryClickCloseButton());
        }

        public void Setup(InputLayerElementData screenStackData)
        {
            presenter.Bind(inputLayerElementView, screenStackData);
        }
    }
}
