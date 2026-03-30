namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public interface ISampleTopPresenter
    {
        TabType CurrentTabType { get; }

        void Setup();
        void OnEnter(TabType targetTabType);
        void OnCompleteInAnimation();
    }
}