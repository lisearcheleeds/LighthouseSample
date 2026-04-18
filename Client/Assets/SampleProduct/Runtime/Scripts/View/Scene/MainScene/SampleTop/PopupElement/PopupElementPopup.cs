using LighthouseExtends.ScreenStack;
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

        public void Setup(PopupElementData screenStackData)
        {
            presenter.Bind(popupElementView, screenStackData);
        }
    }
}
