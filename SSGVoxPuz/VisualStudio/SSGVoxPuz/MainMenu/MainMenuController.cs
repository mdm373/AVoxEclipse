using System.Collections.Generic;
using SSGCore.Utility;
using SSGVoxPuz.LeapAdapt;
using SSGVoxPuz.PuzAudio;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.MainMenu {
    class MainMenuController : SceneSingletonQuickLoadItem<MainMenuController> {
        public AudioClip bgm;
        public PuzMenuScreenComp mainScreen;
        public HandControllerWrapper handControllerPrefab;
        public Transform handMountPoint;

        private SceneLoadSequencer sequencer;
        private Stack<PuzMenuScreenComp> menuStack;
        private PuzMenuControllerComp puzMenuControllerComp;
        private HandControllerWrapper handInstance;

        public override void Load() {
            GameObject handInstanceObject = Instantiate(handControllerPrefab.gameObject);
            handInstance = handInstanceObject.GetComponent<HandControllerWrapper>();
            TransformUtility.ChildAndNormalize(handMountPoint, handInstance.transform);

            menuStack = new Stack<PuzMenuScreenComp>();
            menuStack.Push(mainScreen);
            
            sequencer = SceneLoadSequencer.GetSceneInstance();
            sequencer.OnLoadFinished += HandleLoadFinished;
            puzMenuControllerComp = PuzMenuControllerComp.GetSceneLoadInstance();
            puzMenuControllerComp.SetClosable(false);
        }

        private void HandleLoadFinished(SceneLoadSequencer obj) {
            AudioController.GetSceneInstance().SetBgm(bgm);
            AudioController.GetSceneInstance().isBgmPlaying = true;
            sequencer.OnLoadFinished -= HandleLoadFinished;
            sequencer.OnUnloadStarted += HandleUnloadStarted;
            puzMenuControllerComp.isOpenSilent = true;
            puzMenuControllerComp.ShowMenu(menuStack);
            
        }

        private void HandleUnloadStarted(SceneLoadSequencer arg1, SceneLoadSequencer.UnloadBlockerData arg2) {
            AudioController.GetSceneInstance().isBgmPlaying = false;
            arg1.OnUnloadStarted -= HandleUnloadStarted;
        }

        public override void Unload() {
            
        }
    }
}
