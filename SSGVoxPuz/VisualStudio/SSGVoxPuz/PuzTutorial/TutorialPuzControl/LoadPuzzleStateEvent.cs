using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class LoadPuzzleStateEvent : TutorialEventComp {

        public TextAsset state;

        public override void HandleStarted() {
            PuzController.GetSceneLoadInstance().Faces.Persist.LoadWorld(state);
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
