using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;

namespace SSGVoxPuz.PuzGlobal {
    class SceneCustomDelegatorPersist : CustomBehaviour {

        public bool isPersisted = true;

        public void OnEnable() {
            SceneCustomDelegator delegator = SceneCustomDelegator.GetSceneInstance();
            if (isPersisted) {
                DontDestroyOnLoad(delegator.gameObject);
            }
        }
    }
}
