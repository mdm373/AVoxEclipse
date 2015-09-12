using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    public class QuickLoadPrefabLoader : QuickLoadItem {

        public QuickLoadItem prefab;
        private QuickLoadItem instanced;

        public override void Load() {
            GameObject prefabObject = Instantiate(prefab.gameObject);
            instanced = prefabObject.GetComponent<QuickLoadItem>();
            TransformUtility.ChildAndNormalize(transform, instanced.transform);
            instanced.Load();
        }

        public override void Unload() {
            instanced.Unload();
            DestroyUtility.DestroyAsNeeded(instanced);
        }
    }
}
