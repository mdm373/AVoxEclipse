using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzTutorial.TutorialEvent;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class ZoomDisableEvent  : TutorialEventComp {

        public bool isEnabled;

        public override void HandleStarted() {
            ZoomFace zoomFace = PuzController.GetSceneLoadInstance().Faces.Zoom;
            if (isEnabled) {
                zoomFace.EnableZoom();
            }
            else {
                zoomFace.DisableZoom();
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
