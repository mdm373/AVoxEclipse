using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    public class RotateDisableEvent : TutorialEventComp {

        public bool isEabled;

        public override void HandleStarted() {
            if (isEabled) {
                PuzController.GetSceneLoadInstance().Faces.Rotate.EnableRoation();
            }
            else {
                PuzController.GetSceneLoadInstance().Faces.Rotate.DisableRotation();
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
