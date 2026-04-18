using Cysharp.Threading.Tasks;
using LighthouseExtends.Animation.Runtime;
using LighthouseExtends.ScreenStack;
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

        public void Setup(ButtonElementData screenStackData)
        {
            presenter.Bind(buttonElementView, screenStackData);
        }
    }
}
