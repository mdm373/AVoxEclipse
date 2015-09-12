using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    class DeleteMenuConfirmedMenuOption : PuzMenuOptionHandlerComp{

        public string DeleteFileName { get; set; }
        
        public override void HandleOptionSelect() {
            PersistanceController.GetSceneLoadInstance().DeleteWorld(DeleteFileName);
            PuzMenuControllerComp.GetSceneLoadInstance().NavBack();
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            
        }
    }
}
