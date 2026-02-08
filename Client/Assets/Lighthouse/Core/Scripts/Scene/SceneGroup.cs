using System;
using System.Collections.Generic;
using System.Linq;

namespace Lighthouse.Core.Scene
{
    public sealed class SceneGroup
    {
        public MainSceneId[] MainSceneIds { get; }
        public SceneModuleId[] SceneModuleIds { get; }

        public IReadOnlyDictionary<MainSceneId, SceneModuleId[]> SceneModuleMap { get; }

        public SceneGroup(SceneModuleId[] requireSceneModuleIds, Dictionary<MainSceneId, SceneModuleId[]> sceneModuleMap)
        {
            SceneModuleMap = sceneModuleMap;

            MainSceneIds = sceneModuleMap.Keys
                .Distinct()
                .ToArray();

            SceneModuleIds = requireSceneModuleIds
                .Concat(sceneModuleMap.Values.SelectMany(x => x ?? Array.Empty<SceneModuleId>()))
                .Distinct()
                .ToArray();
        }
    }
}