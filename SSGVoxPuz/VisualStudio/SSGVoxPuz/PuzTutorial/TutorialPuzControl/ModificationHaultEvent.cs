using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class ModificationHaultEvent : TutorialEventComp {
        public override void HandleStarted() {
            PuzController.GetSceneLoadInstance().Faces.Modification.HaultModification();
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
