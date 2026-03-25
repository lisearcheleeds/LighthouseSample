using LighthouseExtends.ScreenStack;
using SampleProduct.View.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace SampleProduct.View.Scene.MainScene.SceneSample1.SceneTransitionDialog
{
    public sealed class SceneTransitionDialog : StandardDialogBase, IScreenStackSetup<SceneTransitionPresenter, SceneTransitionData>
    {
        [FormerlySerializedAs("sceneTransitionPopupView")] [SerializeField] SceneTransitionView sceneTransitionView;

        public void Setup(SceneTransitionPresenter presenter, SceneTransitionData data)
        {
            presenter.Bind(sceneTransitionView, data);
        }
    }
}