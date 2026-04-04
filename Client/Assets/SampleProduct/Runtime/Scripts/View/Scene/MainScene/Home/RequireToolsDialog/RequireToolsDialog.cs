using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;

namespace SampleProduct.View.Scene.MainScene.Home.RequireToolsDialog
{
    public sealed class RequireToolsDialog : StandardDialogBase, IScreenStackSetup<RequireToolsPresenter, RequireToolsData>
    {
        [SerializeField] RequireToolsView dialogSample1View;

        public void Setup(RequireToolsPresenter dialogPresenter, RequireToolsData screenStackData)
        {
            dialogPresenter.Bind(dialogSample1View, screenStackData);
        }
    }
}