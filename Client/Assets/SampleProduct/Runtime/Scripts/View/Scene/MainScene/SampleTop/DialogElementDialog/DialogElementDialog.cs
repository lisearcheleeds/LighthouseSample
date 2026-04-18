using LighthouseExtends.ScreenStack;
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

        public void Setup(DialogElementData screenStackData)
        {
            presenter.Bind(dialogElementView, screenStackData);
        }
    }
}
