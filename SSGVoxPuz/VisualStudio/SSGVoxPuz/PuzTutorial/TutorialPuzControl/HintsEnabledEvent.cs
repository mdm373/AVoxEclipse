using System;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class HintsEnabledEvent : TutorialEventComp {
        public bool isEnabled;

        public override void HandleStarted() {
            HintFace hint = PuzController.GetSceneLoadInstance().Faces.Hint;
            if (isEnabled) {
                hint.EnableHints();
            }
            else {
                hint.DisableHints();
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
