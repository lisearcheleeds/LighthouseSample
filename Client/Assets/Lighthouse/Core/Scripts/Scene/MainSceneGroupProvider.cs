using System;
using System.Collections.Generic;
using System.Linq;

namespace Lighthouse.Core.Scene
{
    public sealed class MainSceneGroupProvider : IMainSceneGroupProvider
    {
        List<MainSceneGroup> mainSceneGroupList = new ();
        Func<IDisposable> enqueueParentLifetimeScope;

        void IMainSceneGroupProvider.SetEnqueueParentLifetimeScope(Func<IDisposable> enqueueParentLifetimeScope)
        {
            this.enqueueParentLifetimeScope = enqueueParentLifetimeScope;
        }

        MainSceneGroup IMainSceneGroupProvider.AddMainSceneGroup(params MainSceneKey[] mainSceneKeys)
        {
            var newGroup = new MainSceneGroup(enqueueParentLifetimeScope, mainSceneKeys);
            mainSceneGroupList.Add(newGroup);
            mainSceneGroupList.Sort((a, b) => a.GroupMainSceneIds.Length.CompareTo(b.GroupMainSceneIds.Length));
            return newGroup;
        }

        MainSceneGroup IMainSceneGroupProvider.GetMainSceneGroup(MainSceneKey mainSceneKey)
        {
            var mainSceneGroup = mainSceneGroupList.FirstOrDefault(mainSceneGroup => mainSceneGroup.GroupMainSceneIds.Contains(mainSceneKey));
            if (mainSceneGroup == null)
            {
                mainSceneGroup = ((IMainSceneGroupProvider)this).AddMainSceneGroup(mainSceneKey);
            }

            return mainSceneGroup;
        }

        void IMainSceneGroupProvider.ClearSceneGroup()
        {
            mainSceneGroupList.Clear();
        }
    }
}