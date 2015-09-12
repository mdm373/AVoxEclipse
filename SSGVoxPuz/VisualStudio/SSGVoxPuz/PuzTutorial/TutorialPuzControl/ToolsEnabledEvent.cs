using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class ToolsEnabledEvent : TutorialEventComp {
        public bool isEnabled = false;
        public override void HandleStarted() {
            if (isEnabled) {
                PuzController.GetSceneLoadInstance().Faces.Tools.EnableTools();
            }
            else {
                PuzController.GetSceneLoadInstance().Faces.Tools.DisableTools();
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
