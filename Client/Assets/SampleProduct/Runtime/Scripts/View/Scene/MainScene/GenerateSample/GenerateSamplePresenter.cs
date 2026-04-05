using VContainer;

namespace SampleProduct.LighthouseGenerated.View.Scene.MainScene.GenerateSample
{
    public sealed class GenerateSamplePresenter : IGenerateSamplePresenter
    {
        IGenerateSampleView generateSampleView;

        [Inject]
        public void Construct(IGenerateSampleView generateSampleView)
        {
            this.generateSampleView = generateSampleView;
        }

        void IGenerateSamplePresenter.Setup()
        {
        }

        void IGenerateSamplePresenter.OnEnter()
        {
        }
    }
}
