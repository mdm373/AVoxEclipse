using System;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    public class PersistSaveMenuOptionHandle : PuzMenuOptionHandlerComp {

        public string previewId = "preview-option";
        private PersistanceController persistanceController;

        public override void HandleOptionSelect() {
            
            string saveName = DateTime.Now.ToString("yyyyMMddHHmmss");
            persistanceController.SaveWorld(saveName);
            

        }


        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            persistanceController = PersistanceController.GetSceneLoadInstance();
        }
    }
}
