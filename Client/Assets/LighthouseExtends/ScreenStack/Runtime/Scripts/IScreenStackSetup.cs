namespace LighthouseExtends.ScreenStack
{
    public interface IScreenStackSetup<in TScreenStackPresenter, in TScreenStackData>
    {
        void Setup(TScreenStackPresenter screenStackPresenter, TScreenStackData screenStackData);
    }
}