using System;
using System.Collections;
using System.Collections.Generic;
using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Persist.Screenshot {
    public class ScreenshotController : SceneSingletonQuickLoadItem<ScreenshotController>, LateCustomUpdater {

        public event Action<ScreenshotController, Texture2D> OnScreenshotTaken;

        private bool isTakingScreenshot;
        private bool isScreenshotRequested;

        public override void Load() {
            SceneCustomDelegator.AddUpdater(this);
        }

        public override void Unload() {
            
        }

        public void RequestScreenshot() {
            if (!isScreenshotRequested) {
                isScreenshotRequested = true;
            }
        }

        public void DoLateUpdate() {
            if (isScreenshotRequested && !isTakingScreenshot) {
                isTakingScreenshot = true;
                isScreenshotRequested = false;
                StartCoroutine(TakeScreenshotCoroutine());
            }
        }

        private IEnumerator<YieldInstruction> TakeScreenshotCoroutine() {
            ScreenshotHideTag[] tagged = FindObjectsOfType<ScreenshotHideTag>();
            foreach(ScreenshotHideTag aTag in tagged)
            {
                aTag.gameObject.SetActive(false);
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Texture2D screenShot = TakeScreenShot();
            FireScreenshotTaken(screenShot);
            isTakingScreenshot = false;
            foreach (ScreenshotHideTag aTag in tagged) {
                aTag.gameObject.SetActive(true);
            }
        }

        private Texture2D TakeScreenShot() {
            Texture2D tex = new Texture2D(Screen.width, Screen.height);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            return tex;
        }

        private void FireScreenshotTaken(Texture2D screenShot) {
            if (OnScreenshotTaken != null) {
                OnScreenshotTaken(this, screenShot);
            }
        }
    }

    
}
