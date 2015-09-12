using System;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using SSGVoxPuz.Tools;

namespace SSGVoxPuz.PuzTutorial.TutorialPuzControl {
    class ToolSetEvent : TutorialEventComp {
        
        public PuzTool tool = PuzTool.Pencil;

        public override void HandleStarted() {
            PuzController.GetSceneLoadInstance().Faces.Tools.SetTool(tool);
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
