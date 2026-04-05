namespace LighthouseExtends.UIComponent.ExclusiveInput
{
    public sealed class ExclusiveInputService : IExclusiveInputService
    {
        int? activePointerId;

        bool IExclusiveInputService.TryUsePointerId(int pointerId)
        {
            if (activePointerId.HasValue)
            {
                return false;
            }
            
            activePointerId = pointerId;
            return true;
        }

        void IExclusiveInputService.ReleasePointerId(int pointerId)
        {
            if (activePointerId == pointerId)
            {
                activePointerId = null;
            }
        }
    }
}
