using System.Runtime.Remoting.Lifetime;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    class PuzMenuOptionCloseHandlerComp : PuzMenuOptionHandlerComp{
        
        public override void HandleOptionSelect() {
            PuzMenuControllerComp controllerComp = PuzMenuControllerComp.GetSceneLoadInstance();
            if (controllerComp.IsBackAvailable()) {
                controllerComp.NavBack();
            } else {
                controllerComp.HideMenu();
            }
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookup) {
            
        }
    }
}
