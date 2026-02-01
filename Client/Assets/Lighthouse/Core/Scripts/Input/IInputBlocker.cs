using System;

namespace Lighthouse.Core
{
    public interface IInputBlocker
    {
        IDisposable Block<T>(bool isSystemLayer = false);
        void UnBlock<T>(bool isSystemLayer = false);
    }
}