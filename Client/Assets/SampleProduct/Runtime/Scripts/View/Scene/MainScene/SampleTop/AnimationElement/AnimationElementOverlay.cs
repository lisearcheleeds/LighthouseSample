using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.AnimationElement
{
    public sealed class AnimationElementOverlay : ProductScreenStackBase, IScreenStackSetup<AnimationElementData>
    {
        [SerializeField] AnimationElementView animationElementView;

        AnimationElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new AnimationElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => animationElementView.TryClickCloseButton());
        }

        public void Setup(AnimationElementData screenStackData)
        {
            presenter.Bind(animationElementView, screenStackData);
        }
    }
}
