using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Interaction {
    class InteractionResetMenuOptionComp : PuzMenuOptionHandlerComp{

        public override void HandleOptionSelect() {
            InteractionGlobalControllerComp.GetSceneLoadInstance().ResetAll();
            PuzMenuControllerComp.GetSceneLoadInstance().HideMenu();
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            
        }
    }
}
