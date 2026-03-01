namespace LighthouseExtends.UI.ExclusiveInput
{
    public sealed class ExclusiveInputService : IExclusiveInputService
    {
        bool IExclusiveInputService.TryUsePointerId(int pointerId)
        {
            return true;
        }

        void IExclusiveInputService.ReleasePointerId(int pointerId)
        {
        }
    }
}