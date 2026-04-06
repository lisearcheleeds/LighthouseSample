using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.DialogElement
{
    public sealed class DialogElementDialog : StandardDialogBase, IScreenStackSetup<DialogElementPresenter, DialogElementData>
    {
        [SerializeField] DialogElementView dialogElementView;

        public void Setup(DialogElementPresenter presenter, DialogElementData screenStackData)
        {
            presenter.Bind(dialogElementView, screenStackData);
        }
    }
}
