using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    public class WaitForMenuShowEvent : BaseWaitEvent {

        public int requiredShowCount = 1;
        public bool isPrimary;
        private int showCount;
        private bool wasShown;
        private MenuFaceMenu menu;
        private bool isShown;


        protected override float GetCurrentValue() {
            return showCount;
        }

        protected override float GetRequiredValue() {
            return requiredShowCount;
        }


        protected override void UpdateTrackedValues() {
            isShown = menu.IsOpen;
            if ( isShown && !wasShown) {
                Debug.Log("ShowCount+");
                showCount++;
            }
            wasShown =  isShown;
        }

        protected override void HandleExtendedStarted() {
            showCount = 0;
            wasShown = false;
            isShown = false;
            menu = isPrimary ? PuzController.GetSceneLoadInstance().Faces.Menu.Primary : PuzController.GetSceneLoadInstance().Faces.Menu.Secondary;
            PuzController.GetSceneLoadInstance().Faces.Menu.HideMenus();
        }

        protected override void HandleExtendedExit() {
            menu = null;
        }

        protected override bool ShouldShake() {
            return isShown && !wasShown;
        }

        protected override bool IsNowFinished() {
            return showCount >= requiredShowCount;
        }
    }
}
