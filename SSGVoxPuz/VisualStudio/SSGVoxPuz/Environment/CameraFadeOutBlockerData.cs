using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Environment {
    public class CameraFadeOutBlockerData : SceneLoadSequencer.UnloadBlocker{
        private readonly GlobalCameraController global;

        public CameraFadeOutBlockerData() {
            global = GlobalCameraController.GetSceneLoadInstance();

        }

        public void PollForFadeFinish() {
            IsBlocked = true;
            global.StartCoroutine(WaitForFadeFinishCoroutine());
        }

        private IEnumerator WaitForFadeFinishCoroutine() {
            while (global.IsFading) {
                yield return new WaitForEndOfFrame();
            }
            IsBlocked = false;
        }
    }
}
