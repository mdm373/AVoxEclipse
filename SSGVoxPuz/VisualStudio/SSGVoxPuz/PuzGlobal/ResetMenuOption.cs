using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    class ResetMenuOption : PuzMenuOptionHandlerComp {
        
        public override void HandleOptionSelect() {
            PuzController.GetSceneLoadInstance().Clear();
            PuzMenuControllerComp.GetSceneLoadInstance().NavBack();
            PuzMenuControllerComp.GetSceneLoadInstance().HideMenu();
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            
        }
    }
}
