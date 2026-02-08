using System;
using System.Collections.Generic;
using System.Linq;

namespace Lighthouse.Core.Scene
{
    public sealed class SceneTransitionDiff
    {
        public bool InSceneGroup { get; }

        public SceneGroup CurrentSceneGroup { get; }
        public MainSceneId CurrentMainSceneId { get; }
        public SceneGroup NextSceneGroup { get; }
        public MainSceneId NextMainSceneId { get; }

        public MainSceneId[] LoadMainSceneIds { get; }
        public MainSceneId[] UnloadMainSceneIds { get; }
        public SceneModuleId[] LoadSceneModuleIds { get; }
        public SceneModuleId[] UnloadSceneModuleIds { get; }

        public SceneTransitionDiff(SceneGroup currentSceneGroup, MainSceneId currentMainSceneId, SceneGroup nextSceneGroup, MainSceneId nextMainSceneId)
        {
            InSceneGroup = ReferenceEquals(currentSceneGroup, nextSceneGroup);

            CurrentSceneGroup = currentSceneGroup;
            CurrentMainSceneId = currentMainSceneId;
            NextSceneGroup = nextSceneGroup;
            NextMainSceneId = nextMainSceneId;

            LoadMainSceneIds = GetMainSceneIdsOnlyInNext(currentSceneGroup, nextSceneGroup);
            UnloadMainSceneIds = GetMainSceneIdsOnlyInCurrent(currentSceneGroup, nextSceneGroup);
            LoadSceneModuleIds = GetSceneModuleIdsOnlyInNext(currentSceneGroup, currentMainSceneId, nextSceneGroup, nextMainSceneId);
            UnloadSceneModuleIds = GetSceneModuleIdsOnlyInCurrent(currentSceneGroup, currentMainSceneId, nextSceneGroup, nextMainSceneId);
        }

        static MainSceneId[] GetMainSceneIdsOnlyInNext(SceneGroup currentSceneGroup, SceneGroup nextSceneGroup)
        {
            var currentMainSceneIds = currentSceneGroup?.MainSceneIds ?? Array.Empty<MainSceneId>();
            var nextMainSceneIds = nextSceneGroup.MainSceneIds;

            var currentSet = new HashSet<MainSceneId>(currentMainSceneIds);

            return nextMainSceneIds
                .Where(m => !currentSet.Contains(m))
                .Distinct()
                .ToArray();
        }

        static MainSceneId[] GetMainSceneIdsOnlyInCurrent(SceneGroup currentSceneGroup, SceneGroup nextSceneGroup)
        {
            var currentMainSceneIds = currentSceneGroup?.MainSceneIds ?? Array.Empty<MainSceneId>();;
            var nextMainSceneIds = nextSceneGroup.MainSceneIds;

            var nextSet = new HashSet<MainSceneId>(nextMainSceneIds);

            return currentMainSceneIds
                .Where(m => !nextSet.Contains(m))
                .Distinct()
                .ToArray();
        }

        static SceneModuleId[] GetSceneModuleIdsOnlyInNext(SceneGroup currentSceneGroup, MainSceneId currentSceneId, SceneGroup nextSceneGroup, MainSceneId nextSceneId)
        {
            var currentSceneModuleIds = currentSceneGroup?.SceneModuleMap[currentSceneId] ?? Array.Empty<SceneModuleId>();
            var nextSceneModuleIds = nextSceneGroup.SceneModuleMap[nextSceneId];

            var currentSet = new HashSet<SceneModuleId>(currentSceneModuleIds);

            return nextSceneModuleIds?.Where(m => !currentSet.Contains(m)).Distinct().ToArray() ?? Array.Empty<SceneModuleId>();
        }

        static SceneModuleId[] GetSceneModuleIdsOnlyInCurrent(SceneGroup currentSceneGroup, MainSceneId currentSceneId, SceneGroup nextSceneGroup, MainSceneId nextSceneId)
        {
            var currentSceneModuleIds = currentSceneGroup?.SceneModuleMap[currentSceneId] ?? Array.Empty<SceneModuleId>();
            var nextSceneModuleIds = nextSceneGroup.SceneModuleMap[nextSceneId] ?? Array.Empty<SceneModuleId>();

            var nextSet = new HashSet<SceneModuleId>(nextSceneModuleIds);

            return currentSceneModuleIds.Where(m => !nextSet.Contains(m)).Distinct().ToArray();
        }
    }
}