using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class ShowMenuEvent :TutorialEventComp {

        public bool isPrimary;

        public override void HandleStarted() {
            MenuFaceMenu menu = isPrimary ? PuzController.GetSceneLoadInstance().Faces.Menu.Primary : PuzController.GetSceneLoadInstance().Faces.Menu.Secondary;
            menu.ShowMenu();
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
