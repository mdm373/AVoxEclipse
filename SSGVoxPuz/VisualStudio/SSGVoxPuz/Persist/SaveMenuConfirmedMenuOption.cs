using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    class SaveMenuConfirmedMenuOption : PuzMenuOptionHandlerComp {

        public string SaveFileName { get; set; }
        
        public override void HandleOptionSelect() {
            PersistanceController.GetSceneLoadInstance().SaveWorld(SaveFileName);
            PuzMenuControllerComp.GetSceneLoadInstance().NavBack();
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            
        }
    }
}
