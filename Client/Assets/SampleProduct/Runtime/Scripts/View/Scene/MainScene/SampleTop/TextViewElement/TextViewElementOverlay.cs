using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.TextViewElement
{
    public sealed class TextViewElementOverlay : ProductScreenStackBase, IScreenStackSetup<TextViewElementData>
    {
        [SerializeField] TextViewElementView textViewElementView;

        TextViewElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new TextViewElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => textViewElementView.TryClickCloseButton());
        }

        public void Setup(TextViewElementData screenStackElementData)
        {
            presenter.Bind(textViewElementView, screenStackElementData);
        }
    }
}
