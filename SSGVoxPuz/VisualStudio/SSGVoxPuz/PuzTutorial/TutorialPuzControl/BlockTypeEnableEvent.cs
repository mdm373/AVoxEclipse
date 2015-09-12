using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class BlockTypeEnableEvent : TutorialEventComp {

        public bool isEnabled = false;
        
        public override void HandleStarted() {
            if (isEnabled) {
                PuzController.GetSceneLoadInstance().Faces.BlockType.EnableBlockTypeChange();
            }
            else {
                PuzController.GetSceneLoadInstance().Faces.BlockType.DisableBlockTypeChange();
            }
        }

        public override void HandleExited() {
            
        }

        public override void HandleUpdated() {
            
        }

        public override void HandleAllFinished() {
            
        }

        public override bool IsBusy() {
            return false;
        }
    }
}
