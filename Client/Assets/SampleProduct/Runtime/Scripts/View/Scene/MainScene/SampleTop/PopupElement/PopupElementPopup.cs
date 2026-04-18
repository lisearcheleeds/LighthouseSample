using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.PopupElement
{
    public sealed class PopupElementPopup : ProductScreenStackBase, IScreenStackSetup<PopupElementData>
    {
        [SerializeField] PopupElementView popupElementView;

        PopupElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new PopupElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => popupElementView.TryClickCloseButton());
        }

        public void Setup(PopupElementData screenStackData)
        {
            presenter.Bind(popupElementView, screenStackData);
        }
    }
}
