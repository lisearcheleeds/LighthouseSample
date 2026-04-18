using Cysharp.Threading.Tasks;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
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

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => dialogSample2View.TryClickCloseButton());
        }

        public void Setup(DialogSample2Data screenStackData)
        {
            presenter.Bind(dialogSample2View, screenStackData);
        }

        public override async UniTask OnEnter(bool isResume)
        {
            await base.OnEnter(isResume);
            await presenter.OnEnter(isResume);
        }
    }
}
