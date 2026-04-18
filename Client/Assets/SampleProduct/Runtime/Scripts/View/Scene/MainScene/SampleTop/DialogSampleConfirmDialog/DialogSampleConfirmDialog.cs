using LighthouseExtends.ScreenStack;
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

        public void Setup(DialogSampleConfirmData screenStackData)
        {
            presenter.Bind(dialogSampleConfirmView, screenStackData);
        }
    }
}
