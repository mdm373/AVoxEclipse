using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using SSGCore.Utility;

namespace SSGVoxPuz.Utility {
    class DestroyTag : CustomBehaviour {

        public void OnEnable() {
            DestroyUtility.DestroyAsNeeded(gameObject);
        }
    }
}
