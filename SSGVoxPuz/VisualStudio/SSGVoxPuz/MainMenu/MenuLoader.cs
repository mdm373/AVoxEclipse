using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.MainMenu {
    class MenuLoader : QuickLoadItem {

        public PuzMenuControllerComp prefab;
        public  bool isClosable;

        public override void Load() {
            GameObject prefabObject = Instantiate(prefab.gameObject);
            PuzMenuControllerComp instanced = prefabObject.GetComponent<PuzMenuControllerComp>();
            TransformUtility.ChildAndNormalize(transform, instanced.transform);
            instanced.SetClosable(isClosable);
            instanced.Load();
        }

        public override void Unload() {
            
        }
    }
}
