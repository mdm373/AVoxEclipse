using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    public class MenuDisableEvent : TutorialEventComp{
        public bool isEnabled;
        public bool isPrimary;


        public override void HandleStarted() {
            if (!isEnabled) {
                if (isPrimary) {
                    PuzController.GetSceneLoadInstance().Faces.Menu.Primary.DisableMenu();
                }
                else {
                    PuzController.GetSceneLoadInstance().Faces.Menu.Secondary.DisableMenu();
                }
            }
            else {
                if (isPrimary) {
                    PuzController.GetSceneLoadInstance().Faces.Menu.Primary.EnableMenu();
                }
                else {
                    PuzController.GetSceneLoadInstance().Faces.Menu.Secondary.EnableMenu();
                }
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
