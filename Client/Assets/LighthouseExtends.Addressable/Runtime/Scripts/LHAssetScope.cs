using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace LighthouseExtends.Addressable
{
    internal sealed class LHAssetScope : ILHAssetScope
    {
        readonly LHAssetManager manager;
        readonly List<IDisposable> handles = new();
        bool disposed;

        internal LHAssetScope(LHAssetManager manager)
        {
            this.manager = manager;
        }

        public async UniTask<T> LoadAsync<T>(string address, CancellationToken ct = default)
            where T : UnityEngine.Object
        {
            if (disposed) throw new ObjectDisposedException(nameof(LHAssetScope));

            var handle = await manager.LoadInternalAsync<T>(address, ct);

            // Scope may have been disposed while awaiting; release the handle immediately.
            if (disposed)
            {
                handle.Dispose();
                throw new ObjectDisposedException(nameof(LHAssetScope));
            }

            handles.Add(handle);
            return handle.Asset;
        }

        public async UniTask<IAssetHandle<T>> LoadAssetAsync<T>(string address, CancellationToken ct = default)
            where T : UnityEngine.Object
        {
            if (disposed) throw new ObjectDisposedException(nameof(LHAssetScope));

            var handle = await manager.LoadInternalAsync<T>(address, ct);

            if (disposed)
            {
                handle.Dispose();
                throw new ObjectDisposedException(nameof(LHAssetScope));
            }

            handles.Add(handle);
            return handle;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            foreach (var handle in handles)
                handle.Dispose();

            handles.Clear();
        }
    }
}
