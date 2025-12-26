using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSampleConfirmDialog
{
    public sealed class DialogSampleConfirmDialog : StandardDialogBase, IScreenStackSetup<DialogSampleConfirmData>
    {
        [SerializeField] DialogSampleConfirmView dialogSampleConfirmView;

        DialogSampleConfirmPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new DialogSampleConfirmPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => dialogSampleConfirmView.TryClickCloseButton());
        }

        public void Setup(DialogSampleConfirmData screenStackData)
        {
            presenter.Bind(dialogSampleConfirmView, screenStackData);
        }
    }
}
