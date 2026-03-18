using LighthouseExtends.Popup;
using SampleProduct.View.Popup;
using UnityEngine;
namespace SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionPopup
{
    public sealed class SceneTransitionPopup : StandardPopupBase, IPopupSetup<SceneTransitionPopupPresenter, SceneTransitionPopupData>
    {
        [SerializeField] SceneTransitionPopupView sceneTransitionPopupView;

        public void Setup(SceneTransitionPopupPresenter popupPresenter, SceneTransitionPopupData popupData)
        {
            popupPresenter.Bind(sceneTransitionPopupView, popupData);
        }
    }
}