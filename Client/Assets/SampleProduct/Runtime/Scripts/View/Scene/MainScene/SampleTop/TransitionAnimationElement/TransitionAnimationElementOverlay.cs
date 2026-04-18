using LighthouseExtends.ScreenStack;
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

        public void Setup(TransitionAnimationElementData screenStackData)
        {
            presenter.Bind(transitionAnimationElementView, screenStackData);
        }
    }
}
