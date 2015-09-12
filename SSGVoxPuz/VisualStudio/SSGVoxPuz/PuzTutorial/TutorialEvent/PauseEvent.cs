using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSGVoxPuz.PuzTutorial.TutorialEvent {
    public class PauseEvent : TutorialEventComp {

        public bool isBusy = true;

        public override void HandleStarted() {
            
        }

        public override void HandleExited() {
            
        }

        public override void HandleUpdated() {
            
        }

        public override void HandleAllFinished() {
            
        }

        public override bool IsBusy() {
            return isBusy;
        }
    }
}
