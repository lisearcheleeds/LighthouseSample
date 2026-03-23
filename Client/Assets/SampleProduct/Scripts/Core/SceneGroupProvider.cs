using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lighthouse.Scene;

namespace SampleProduct.Core
{
    public sealed class SceneGroupProvider : ISceneGroupProvider
    {
        static readonly ModuleSceneId[] RequireSceneModuleIds =
        {
            SampleProductModuleSceneId.Overlay,
            SampleProductModuleSceneId.Popup
        };

        static readonly IReadOnlyDictionary<MainSceneId, ModuleSceneId[]> SceneModuleMap = new Dictionary<MainSceneId, ModuleSceneId[]>()
            {
                { SampleProductMainSceneId.Splash, null },
                { SampleProductMainSceneId.Title, null },
                { SampleProductMainSceneId.Home, new[] { SampleProductModuleSceneId.Background }},
                { SampleProductMainSceneId.SampleTop, new[] { SampleProductModuleSceneId.Background, SampleProductModuleSceneId.GlobalHeader }},
                { SampleProductMainSceneId.SceneSample1, new[] { SampleProductModuleSceneId.Background, SampleProductModuleSceneId.GlobalHeader }},
                { SampleProductMainSceneId.SceneSample2, new[] { SampleProductModuleSceneId.Background, SampleProductModuleSceneId.GlobalHeader }},
                { SampleProductMainSceneId.SceneSample3, new[] { SampleProductModuleSceneId.Background, SampleProductModuleSceneId.GlobalHeader }},
                { SampleProductMainSceneId.Edit, null },
            };

        static readonly MainSceneId[][] MainSceneGroupList =
        {
            new[] { SampleProductMainSceneId.Splash, SampleProductMainSceneId.Title },
            new[] { SampleProductMainSceneId.Home, SampleProductMainSceneId.Edit },
            new[] { SampleProductMainSceneId.SampleTop, SampleProductMainSceneId.SceneSample1, SampleProductMainSceneId.SceneSample2, SampleProductMainSceneId.SceneSample3 }
        };

        static readonly SceneGroup[] SceneGroupList = CreateSceneGroups();

        SceneGroup ISceneGroupProvider.GetSceneGroup(MainSceneId mainSceneId)
        {
            return SceneGroupList.First(sceneGroup => sceneGroup.MainSceneIds.Contains(mainSceneId));
        }

        static SceneGroup[] CreateSceneGroups()
        {
#if UNITY_EDITOR
            var duplicateMainScenes = MainSceneGroupList
                .SelectMany(x => x.Select(y => y))
                .GroupBy(x => x.Name)
                .Where(x => x.Count() != 1)
                .Select(x => x.Key)
                .ToArray();
            if (duplicateMainScenes.Any())
            {
                var duplicateSceneNames = string.Join(", ", duplicateMainScenes);
                throw new ConstraintException($"Duplicate scenes {duplicateSceneNames}");
            }
#endif

            return MainSceneGroupList
                .Select(CreateSceneGroup)
                .ToArray();
        }

        static SceneGroup CreateSceneGroup(MainSceneId[] mainSceneKeyList)
        {
            var sceneModuleDic = mainSceneKeyList.ToDictionary(
                mainSceneKey => mainSceneKey,
                mainSceneKey => RequireSceneModuleIds.Concat(SceneModuleMap[mainSceneKey] ?? Array.Empty<ModuleSceneId>()).ToArray());

            return new SceneGroup(sceneModuleDic);
        }
    }
}