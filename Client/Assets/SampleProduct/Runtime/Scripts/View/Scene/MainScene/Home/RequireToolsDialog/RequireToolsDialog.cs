using Cysharp.Threading.Tasks;
using LighthouseExtends.InputLayer;
using LighthouseExtends.ScreenStack;
using SampleProduct.Input;
using SampleProduct.Input.Layer;
using SampleProduct.View.Base;
using UnityEngine;
using VContainer;

namespace SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog
{
    public sealed class RequireToolsDialog : StandardDialogBase, IScreenStackSetup<RequireToolsData>
    {
        [SerializeField] RequireToolsView dialogView;

        RequireToolsPresenter presenter;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            presenter = new RequireToolsPresenter();
            objectResolver.Inject(presenter);
        }

        protected override IInputLayer CreateInputLayer(InputActions inputActions)
        {
            return new DefaultScreenStackInputLayer(inputActions, () => dialogView.TryClickCloseButton());
        }

        public void Setup(RequireToolsData screenStackData)
        {
            presenter.Bind(dialogView, screenStackData);
        }
    }
}
