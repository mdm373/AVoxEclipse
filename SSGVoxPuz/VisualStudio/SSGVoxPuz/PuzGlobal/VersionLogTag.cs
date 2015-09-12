using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    class VersionLogTag : QuickLoadItem {
        public override void Load() {
            Debug.Log("Build Version: " + Versioning.GetBuildLongVersion());
        }

        public override void Unload() {
            
        }
    }
}
