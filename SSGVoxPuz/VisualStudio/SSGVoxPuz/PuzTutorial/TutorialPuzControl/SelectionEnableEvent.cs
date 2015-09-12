using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    public class SelectionEnableEvent : TutorialEventComp {

        public bool isEnabled;
        
        public override void HandleStarted() {
            if (isEnabled) {
                PuzController.GetSceneLoadInstance().Faces.Selection.EnableSelection();
            }
            else {
                PuzController.GetSceneLoadInstance().Faces.Selection.DisableSelection();
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
