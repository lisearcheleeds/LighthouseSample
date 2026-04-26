using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LighthouseExtends.Addressable
{
    /// <summary>
    /// Implements ILHAssetManager. Shares a single Addressables handle per address with ref-counting;
    /// Addressables.Release is called only when all holders have released.
    /// </summary>
    public sealed class LHAssetManager : ILHAssetManager, IDisposable
    {
        class Entry
        {
            public AsyncOperationHandle Handle { get; set; }
            public int RefCount { get; set; }
        }

        readonly Dictionary<string, Entry> entries = new();

        bool disposed;

        public async UniTask<LHAssetHandle<T>> LoadInternalAsync<T>(string address, CancellationToken ct) where T : UnityEngine.Object
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(LHAssetManager));
            }

            if (!entries.TryGetValue(address, out var entry))
            {
                entry = new Entry
                {
                    Handle = Addressables.LoadAssetAsync<T>(address),
                    RefCount = 0,
                };
                entries[address] = entry;
            }

            // Increment before await so concurrent calls to the same address accumulate RefCount correctly.
            entry.RefCount++;

            try
            {
                // ct cancels this await but not the underlying Addressables load; other callers may share the same handle.
                await entry.Handle.ToUniTask(cancellationToken: ct);
                return new LHAssetHandle<T>((T)entry.Handle.Result, () => Release(address));
            }
            catch
            {
                Release(address);
                throw;
            }
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            foreach (var entry in entries.Values)
            {
                Addressables.Release(entry.Handle);
            }

            entries.Clear();
        }

        ILHAssetScope ILHAssetManager.CreateScope()
        {
            return new LHAssetScope(this);
        }

        void Release(string address)
        {
            if (!entries.TryGetValue(address, out var entry))
            {
                return;
            }

            entry.RefCount--;

            if (entry.RefCount <= 0)
            {
                Addressables.Release(entry.Handle);
                entries.Remove(address);
            }
        }
    }
}
