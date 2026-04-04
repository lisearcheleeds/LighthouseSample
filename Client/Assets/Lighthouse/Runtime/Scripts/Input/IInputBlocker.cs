using System;

namespace Lighthouse.Input
{
    public interface IInputBlocker
    {
        IDisposable Block<T>(bool isSystemLayer = false);
        void UnBlock<T>(bool isSystemLayer = false);
    }
}