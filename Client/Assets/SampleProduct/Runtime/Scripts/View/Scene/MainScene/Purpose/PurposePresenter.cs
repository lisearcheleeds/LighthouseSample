using VContainer;

namespace SampleProduct.View.Scene.MainScene.Purpose
{
    public sealed class PurposePresenter : IPurposePresenter
    {
        IPurposeView purposeView;

        [Inject]
        public void Construct(IPurposeView purposeView)
        {
            this.purposeView = purposeView;
        }

        void IPurposePresenter.Setup()
        {
        }

        void IPurposePresenter.OnEnter()
        {
        }
    }
}
