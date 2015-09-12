using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    public class PivotEnableEvent : TutorialEventComp {

        public bool isEnabled;
        
        public override void HandleStarted() {
            if (isEnabled) {
                PuzController.GetSceneLoadInstance().Faces.Pivot.EnablePivoting();
            }
            else {
                PuzController.GetSceneLoadInstance().Faces.Pivot.DisablePivoting();
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
