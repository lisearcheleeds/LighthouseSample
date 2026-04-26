using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Addressable
{
    /// <summary>
    /// Tracks a set of loaded assets and releases them all when disposed.
    /// Use LoadAssetAsync instead of LoadAsync when early release before scope disposal is needed.
    /// </summary>
    public interface ILHAssetScope : IDisposable
    {
        UniTask<T> LoadAsync<T>(string address, CancellationToken ct = default) where T : UnityEngine.Object;

        UniTask<IAssetHandle<T>> LoadAssetAsync<T>(string address, CancellationToken ct = default) where T : UnityEngine.Object;
    }
}
