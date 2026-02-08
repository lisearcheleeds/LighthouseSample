namespace Lighthouse.Core.Scene.SceneBase
{
    public abstract class SceneModuleBase : SceneBase
    {
        public abstract SceneModuleId SceneModuleId { get; }

        public VisibleStateType VisibleStateType { get; protected set; }

        public virtual bool IsAlwaysInAnimation { get; protected set; } = false;
        public virtual bool IsAlwaysOutAnimation { get; protected set; } = false;

        protected override void OnBeginInAnimation(TransitionType transitionType, bool withStateChange)
        {
            base.OnBeginInAnimation(transitionType, withStateChange);

            VisibleStateType = VisibleStateType.Showing;
        }

        protected override void OnCompleteInAnimation(TransitionType transitionType, bool withStateChange)
        {
            VisibleStateType = VisibleStateType.Visible;
        }

        protected override void OnBeginOutAnimation(TransitionType transitionType, bool withStateChange)
        {
            VisibleStateType = VisibleStateType.Hiding;
        }

        protected override void OnCompleteOutAnimation(TransitionType transitionType, bool withStateChange)
        {
            base.OnCompleteOutAnimation(transitionType, withStateChange);

            VisibleStateType = VisibleStateType.Hidden;
        }

        public virtual void OnSceneTransitionFinished(MainSceneId mainSceneId)
        {
        }
    }
}