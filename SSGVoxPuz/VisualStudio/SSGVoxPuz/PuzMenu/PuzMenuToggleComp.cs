using System.Collections.Generic;
using SSGCore.Custom;

namespace SSGVoxPuz.PuzMenu {
    class PuzMenuToggleComp : CustomBehaviour {

        public bool showWithMenu = false;
        private PuzMenuControllerComp controllerComp;

        public void OnEnable() {
            controllerComp = PuzMenuControllerComp.GetSceneLoadInstance();
            if (controllerComp != null) {
                controllerComp.OnShow -= HandleShow;
                controllerComp.OnShow += HandleShow;
                controllerComp.OnHide -= HandleHide;
                controllerComp.OnHide += HandleHide;
                if (!controllerComp.IsShown) {
                    HandleHide(controllerComp);
                }
            }
        }

        public void OnDestroy() {
            if (controllerComp != null) {
                controllerComp.OnShow -= HandleShow;
                controllerComp.OnHide -= HandleHide;   
            }
        }

        private void HandleHide(PuzMenuControllerComp menu) {
            gameObject.SetActive(!showWithMenu);
        }

        private void HandleShow(PuzMenuControllerComp menu) {
            gameObject.SetActive(showWithMenu);
        }
    }
}
