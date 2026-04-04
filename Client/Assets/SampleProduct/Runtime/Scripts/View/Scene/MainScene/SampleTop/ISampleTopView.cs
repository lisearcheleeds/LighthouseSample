using System;

namespace SampleProduct.View.Scene.MainScene.SampleTop
{
    public interface ISampleTopView
    {
        public IDisposable SubscribeBackSceneButtonClick(Action action);

        public IDisposable SubscribeOverviewTabButtonClick(Action action);
        public IDisposable SubscribeSceneSampleTabButtonClick(Action action);
        public IDisposable SubscribeDialogSampleTabButtonClick(Action action);
        public IDisposable SubscribeElementTabButtonClick(Action action);
        public IDisposable SubscribeInGameTabButtonClick(Action action);

        public IDisposable SubscribeOverviewNextPageButtonClick(Action action);
        public IDisposable SubscribeOverviewPrevPageButtonClick(Action action);
        public IDisposable SubscribeOpenGitHubButtonClick(Action action);

        public IDisposable SubscribeSceneSample1ButtonClick(Action action);

        public IDisposable SubscribeDialogSample1ButtonClick(Action action);
        public IDisposable SubscribeDialogSample2ButtonClick(Action action);

        public IDisposable SubscribeGame1ButtonClick(Action action);
        public IDisposable SubscribeGame2ButtonClick(Action action);

        void ResetView();
        void SwitchTab(TabType prevTabType, TabType tabType);
        void SetOverviewIndex(int prevIndex, int index);
        void SetOverviewButtonState(bool isNextButtonActive, bool isPrevButtonActive);
    }
}