using System;

namespace Lighthouse.Core.Scene
{
    public interface IMainSceneGroupProvider
    {
        void SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope);

        MainSceneGroup AddMainSceneGroup(params MainSceneKey[] mainSceneKeys);
        MainSceneGroup GetMainSceneGroup(MainSceneKey mainSceneKey);
        void ClearSceneGroup();
    }
}