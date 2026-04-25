using System;

namespace LighthouseExtends.Addressable
{
    public interface IAssetHandle<out T> : IDisposable
    {
        T Asset { get; }
    }
}
