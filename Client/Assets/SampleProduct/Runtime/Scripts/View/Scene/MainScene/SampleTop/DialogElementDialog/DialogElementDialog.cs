using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogElementDialog
{
    public sealed class DialogElementDialog : StandardDialogBase, IScreenStackSetup<DialogElementData>
    {
        [SerializeField] DialogElementView dialogElementView;

        DialogElementPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new DialogElementPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => dialogElementView.TryClickCloseButton());
        }

        public void Setup(DialogElementData screenStackData)
        {
            presenter.Bind(dialogElementView, screenStackData);
        }
    }
}
