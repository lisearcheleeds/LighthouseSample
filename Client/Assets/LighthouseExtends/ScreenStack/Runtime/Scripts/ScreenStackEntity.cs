namespace LighthouseExtends.ScreenStack
{
    public sealed class ScreenStackEntity
    {
        public IScreenStack ScreenStack { get; }
        public IScreenStackPresenter ScreenStackPresenter { get; }
        public IScreenStackData ScreenStackData { get; }

        public ScreenStackEntity(IScreenStack screenStack, IScreenStackPresenter screenStackPresenter, IScreenStackData screenStackData)
        {
            ScreenStack = screenStack;
            ScreenStackPresenter = screenStackPresenter;
            ScreenStackData = screenStackData;
        }
    }
}