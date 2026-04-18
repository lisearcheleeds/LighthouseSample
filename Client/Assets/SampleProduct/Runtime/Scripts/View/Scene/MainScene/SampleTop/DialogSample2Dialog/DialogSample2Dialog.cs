using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample2Dialog
{
    public sealed class DialogSample2Dialog : StandardDialogBase, IScreenStackSetup<DialogSample2Data>
    {
        [SerializeField] DialogSample2View dialogSample2View;

        DialogSample2Presenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new DialogSample2Presenter();
            objectResolver.Inject(presenter);
        }

        public void Setup(DialogSample2Data screenStackData)
        {
            presenter.Bind(dialogSample2View, screenStackData);
        }

        public override UniTask OnEnter(bool isResume)
        {
            return presenter.OnEnter(isResume);
        }
    }
}
