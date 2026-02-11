using System.Collections.Generic;
using System.Linq;
using Lighthouse.Core.Scene;
using ProductNameSpace;
using VContainer;

namespace SampleProduct.Core
{
    public sealed class SceneGroupProvider : ISceneGroupProvider
    {
        List<SceneGroup> sceneGroupList = new ();

        public ModuleSceneId[] RequireSceneModuleIds = new[] { SampleProductSceneModuleId.Overlay };

        public Dictionary<MainSceneId, ModuleSceneId[]> SceneModuleMap = new Dictionary<MainSceneId, ModuleSceneId[]>()
            {
                { SampleProductMainSceneId.Splash, null },
                { SampleProductMainSceneId.Title, null },
                { SampleProductMainSceneId.Home, new[] { SampleProductSceneModuleId.Background, SampleProductSceneModuleId.GlobalHeader }},
                { SampleProductMainSceneId.Edit, null },
            };

        [Inject]
        public SceneGroupProvider()
        {
            sceneGroupList.Add(CreateSceneGroup(SampleProductMainSceneId.Splash, SampleProductMainSceneId.Title));
            sceneGroupList.Add(CreateSceneGroup(SampleProductMainSceneId.Home, SampleProductMainSceneId.Edit));
        }

        SceneGroup ISceneGroupProvider.GetSceneGroup(MainSceneId mainSceneId)
        {
            return sceneGroupList.First(sceneGroup => sceneGroup.MainSceneIds.Contains(mainSceneId));
        }

        SceneGroup CreateSceneGroup(params MainSceneId[] mainSceneKeyList)
        {
            return new SceneGroup(
                RequireSceneModuleIds,
                mainSceneKeyList.ToDictionary(mainSceneKey => mainSceneKey, mainSceneKey => SceneModuleMap[mainSceneKey]));
        }
    }
}