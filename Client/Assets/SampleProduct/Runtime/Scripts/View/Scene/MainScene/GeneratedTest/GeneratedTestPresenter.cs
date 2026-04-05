using VContainer;

namespace SampleProduct.View.Scene.MainScene.GeneratedTest
{
    public sealed class GeneratedTestPresenter : IGeneratedTestPresenter
    {
        IGeneratedTestView generatedTestView;

        [Inject]
        public void Construct(IGeneratedTestView generatedTestView)
        {
            this.generatedTestView = generatedTestView;
        }

        void IGeneratedTestPresenter.Setup()
        {
        }

        void IGeneratedTestPresenter.OnEnter()
        {
        }
    }
}
