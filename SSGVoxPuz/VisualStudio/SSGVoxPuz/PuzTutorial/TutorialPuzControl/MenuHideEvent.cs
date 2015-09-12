using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class MenuHideEvent : TutorialEventComp {
        public override void HandleStarted() {
            PuzController.GetSceneLoadInstance().Faces.Menu.HideMenus();
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
