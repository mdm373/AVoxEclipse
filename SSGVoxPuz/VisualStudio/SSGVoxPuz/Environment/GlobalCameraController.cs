using System;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.SceneNavigation;
using UnityEngine;

namespace SSGVoxPuz.Environment {
    public class GlobalCameraController : SceneSingletonQuickLoadItem<GlobalCameraController> {

        public event Action<GlobalCameraController> OnSceneFadeInFinish;
        public CustomCamera cameraPrefab;
        public CustomCamera loadingCameraPrefab;
        private CustomCamera sceneCamera;
        private CustomCamera loadingCamera;
        private CustomCamera activeCamera;
        
        public bool IsFading {
            get { return activeCamera.IsFading; }
        }

        public void SendToLoadScreen() {
            sceneCamera.OnFadeFinish += HandleSceneFadeOut;
            sceneCamera.SetCameraActive(false);
        }

        public void HookToSceneMountPoint(Transform parent) {
            SceneNavigationController.GetSceneLoadInstance().AddTransitionBlocker(this);
            TransformUtility.ChildAndNormalize(parent, sceneCamera.transform);
            loadingCamera.OnFadeFinish += HandleLoadFadeOut;
            loadingCamera.SetCameraActive(false);

        }

        private void HandleSceneFadeOut(CustomCamera aCamera) {
            TransformUtility.ChildAndNormalize(transform, sceneCamera.transform);
            aCamera.OnFadeFinish -= HandleSceneFadeOut;
            ActivateLoadingCamera();
        }

        private void HandleLoadFadeOut(CustomCamera obj) {
            obj.OnFadeFinish -= HandleLoadFadeOut;
            ActiveateSceneCamera();
        }

        public GameObject GetActiveCameraObject() {
            return null;
        }

        public override void Load() {
            GameObject  sceneCameraObject = Instantiate(cameraPrefab.gameObject);
            GameObject loadingCameraObject = Instantiate(loadingCameraPrefab.gameObject);
            TransformUtility.ChildAndNormalize(transform, sceneCameraObject.transform);
            TransformUtility.ChildAndNormalize(transform, loadingCameraObject.transform);
            sceneCamera = sceneCameraObject.GetComponent<CustomCamera>();
            loadingCamera = loadingCameraObject.GetComponent<CustomCamera>();

            sceneCamera.Load();
            loadingCamera.Load();

            sceneCamera.ForceOff();
            loadingCamera.SetCameraActive(false);
            ActivateLoadingCamera();
        }

        private void ActivateLoadingCamera() {
            activeCamera = loadingCamera;
            loadingCamera.SetCameraActive(true);
        }

        private void ActiveateSceneCamera() {
            activeCamera = sceneCamera;
            sceneCamera.SetCameraActive(true);
            sceneCamera.OnFadeFinish += HandleSceneFadeIn;
        }

        private void HandleSceneFadeIn(CustomCamera obj) {
            SceneNavigationController.GetSceneLoadInstance().RemoveTransitionBlocker(this);
            sceneCamera.OnFadeFinish -= HandleSceneFadeIn;
            if (OnSceneFadeInFinish != null) {
                OnSceneFadeInFinish(this);
            }

        }

        public override void Unload() {
            sceneCamera.Unload();
            loadingCamera.Unload();
        }

        public void HideSkyBox() {
            sceneCamera.SetSkyBoxEnabled(false);
        }

        public void ShowSkyBox() {
            sceneCamera.SetSkyBoxEnabled(true);
        }
    }
}
