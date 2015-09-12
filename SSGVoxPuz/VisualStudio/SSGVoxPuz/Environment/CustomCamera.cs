using System;
using System.Collections.Generic;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Environment {
    public class CustomCamera : QuickLoadItem {
        public QuickLoadItemList quickLoadList;
        public List<GameObject> cameras;
        public CameraVeilFade fade;
        public Camera primaryCamera;
        private bool isCameraActive;
        private Skybox skybox;
        public bool IsFading { get { return (fade != null) && fade.IsFading; } }

        public event Action<CustomCamera> OnFadeFinish;

        public void SetCameraActive(bool isActive) {
            isCameraActive = isActive;
            if (isActive) {
                for (int i = 0; i < cameras.Count; i++) {
                    cameras[i].SetActive(true);
                }   
            }
            if(fade != null) {
                fade.OnFadeFinish += HandleFadeFinish;
            }
            if (isActive && fade != null) {
                fade.FadeIn();
            }
            else if (!isActive && fade != null) {
                fade.FadeToBlack();
            }
            if (fade == null) {
                FireFadeFinish(isActive);
            }  
        }

        private void HandleFadeFinish(CameraVeilFade aFade) {
            aFade.OnFadeFinish -= HandleFadeFinish;
            FireFadeFinish(isCameraActive);
        }

        private void FireFadeFinish(bool isActive) {
            if (!isActive) {
                for (int i = 0; i < cameras.Count; i++) {
                    cameras[i].SetActive(false);
                }
            }
            if (OnFadeFinish != null) {
                OnFadeFinish(this);
            }
        }

        public void ForceOff() {
            for (int i = 0; i < cameras.Count; i++) {
                cameras[i].SetActive(false);
            }
        }

        public override void Load() {
            skybox = GetComponentInChildren<Skybox>();
            quickLoadList.Load();
        }

        public override void Unload() {
            quickLoadList.Unload();
        }

        public void SetSkyBoxEnabled(bool isEnabled) {
            skybox.enabled = isEnabled;
            if (primaryCamera != null) {
                primaryCamera.clearFlags = !isEnabled ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
            }
        }

    }
}
