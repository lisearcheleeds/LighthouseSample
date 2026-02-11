using System;
using System.Collections.Generic;
using System.Linq;

namespace Lighthouse.Core.Scene
{
    public sealed class SceneGroup
    {
        public MainSceneId[] MainSceneIds { get; }
        public ModuleSceneId[] SceneModuleIds { get; }

        public IReadOnlyDictionary<MainSceneId, ModuleSceneId[]> SceneModuleMap { get; }

        public SceneGroup(ModuleSceneId[] requireSceneModuleIds, Dictionary<MainSceneId, ModuleSceneId[]> sceneModuleMap)
        {
            SceneModuleMap = sceneModuleMap;

            MainSceneIds = sceneModuleMap.Keys
                .Distinct()
                .ToArray();

            SceneModuleIds = requireSceneModuleIds
                .Concat(sceneModuleMap.Values.SelectMany(x => x ?? Array.Empty<ModuleSceneId>()))
                .Distinct()
                .ToArray();
        }
    }
}