using System.Runtime.Remoting.Messaging;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Environment {

    public class SceneCameraFadeInFadeOut : SceneSingletonSceneLoadItem<SceneCameraFadeInFadeOut> {

        public  Transform cameraMount;
        
        private GlobalCameraController globalCamera;
        private SceneLoadSequencer sceneLoadSequence;


        public override void Load() {
            globalCamera = GlobalCameraController.GetSceneLoadInstance();
            sceneLoadSequence = SceneLoadSequencer.GetSceneInstance();
            SceneLoadSequencer.GetSceneInstance();
            sceneLoadSequence.OnLoadFinished += HandleLoadFinished;
        }

        private void HandleLoadFinished(SceneLoadSequencer obj) {
            sceneLoadSequence.OnLoadFinished -= HandleLoadFinished;
            sceneLoadSequence.OnUnloadStarted += HandleUnloadStarted;
            globalCamera.HookToSceneMountPoint(cameraMount);
        }

        private void HandleUnloadStarted(SceneLoadSequencer sequencer, SceneLoadSequencer.UnloadBlockerData blockerData) {
            sceneLoadSequence.OnUnloadStarted -= HandleUnloadStarted;
            CameraFadeOutBlockerData cameraBlocker = new CameraFadeOutBlockerData();
            blockerData.blockers.Push(cameraBlocker);
            globalCamera.SendToLoadScreen();
            cameraBlocker.PollForFadeFinish();
        }

        public override void Unload() {
            
        }

        public override bool IsLoaded {
            get { return true; }
        }

        public override bool IsUnloaded {
            get { return true; }
        }

        public override void OnNextItemLoading() {

        }

        public override void OnNexuItemUnloading() {
            
        }
    }
}
