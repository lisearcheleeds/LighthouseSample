using Cysharp.Threading.Tasks;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.SampleTop.DialogSample1Dialog
{
    public sealed class DialogSample1Dialog : StandardDialogBase, IScreenStackSetup<DialogSample1Data>
    {
        [SerializeField] DialogSample1View dialogSample1View;

        DialogSample1Presenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new DialogSample1Presenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => dialogSample1View.TryClickCloseButton());
        }

        public void Setup(DialogSample1Data screenStackData)
        {
            presenter.Bind(dialogSample1View, screenStackData);
        }

        public override async UniTask OnEnter(bool isResume)
        {
            await base.OnEnter(isResume);
            await presenter.OnEnter(isResume);
        }
    }
}
