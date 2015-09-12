using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {

    public class SceneWalker : CustomBehaviour {

        public void OnEnable() {
            DontDestroyOnLoad(gameObject);
        }


    }
}
