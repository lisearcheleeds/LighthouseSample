using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighthouse.Core.Scene
{
    public abstract class TransitionDataBase
    {
        public abstract MainSceneKey MainSceneKey { get; }

        public CommonSceneKey[] RequireCommonSceneIds
        {
            get
            {
                if (requireCommonSceneIds == null)
                {
                    requireCommonSceneIds = MustRequireCommonSceneIds.Concat(ExtendCommonSceneIds).ToArray();
                }

                return requireCommonSceneIds;
            }
        }

        protected virtual CommonSceneKey[] MustRequireCommonSceneIds { get; } = Array.Empty<CommonSceneKey>();
        protected virtual CommonSceneKey[] ExtendCommonSceneIds { get; } = Array.Empty<CommonSceneKey>();

        CommonSceneKey[] requireCommonSceneIds;

        public virtual bool CanTransition()
        {
            return true;
        }

        public virtual UniTask LoadSceneState(TransitionType transitionType, CancellationToken cancelToken)
        {
            return UniTask.CompletedTask;
        }
    }
}