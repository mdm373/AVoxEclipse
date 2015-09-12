using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Persist.Screenshot {
    class ScreenShotKeyListener : QuickLoadItem, CustomUpdater{
        private ScreenshotController controller;
        public KeyCode screenCapButton = KeyCode.Home;

        public override void Load() {
            controller = ScreenshotController.GetSceneLoadInstance();
            SceneCustomDelegator.AddUpdater(this);
        }

        public override void Unload() {
            SceneCustomDelegator.RemoveUpater(this);
        }

        public void DoUpdate() {
            if (Input.GetKeyDown(screenCapButton)) {
                controller.RequestScreenshot();
            }
        }
    }
}
