using Cysharp.Threading.Tasks;
using LighthouseExtends.ScreenStack;
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

        public void Setup(RequireToolsData screenStackData)
        {
            presenter.Bind(dialogView, screenStackData);
        }

        public override UniTask OnEnter(bool isResume)
        {
            return presenter.OnEnter(isResume);
        }

        public override UniTask OnLeave()
        {
            return presenter.OnLeave();
        }
    }
}
