using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.TransitionAnimationElement
{
    public sealed class TransitionAnimationElementOverlay : ProductScreenStackBase, IScreenStackSetup<TransitionAnimationElementData>
    {
        [SerializeField] TransitionAnimationElementView transitionAnimationElementView;

        TransitionAnimationElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new TransitionAnimationElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => transitionAnimationElementView.TryClickCloseButton());
        }

        public void Setup(TransitionAnimationElementData screenStackData)
        {
            presenter.Bind(transitionAnimationElementView, screenStackData);
        }
    }
}
