using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;

namespace SSGVoxPuz.PuzGlobal.SceneNavigation {
    class SceneNavigatorEnableTag : CustomBehaviour{
        protected override void HandleCacheRequest() {
            SceneNavigationController.GetSceneLoadInstance().FlagSceneChanged();
        }
    }
}
