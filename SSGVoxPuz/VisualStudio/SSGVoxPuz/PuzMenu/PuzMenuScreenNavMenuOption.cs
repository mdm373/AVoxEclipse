using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    public class PuzMenuScreenNavMenuOption : PuzMenuOptionHandlerComp {

        public PuzMenuScreenComp toNavTo;

        public override void HandleOptionSelect() {
            PuzMenuControllerComp.GetSceneLoadInstance().NavToMenu(toNavTo);
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup option) {
            
        }
    }
}
