using SSGCore.Custom;
using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.Environment;
using SSGVoxPuz.LeapAdapt;
using SSGVoxPuz.PuzAudio;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.SceneNavigation;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.Welcome {
    public class WelcomeScreenController : SceneSingletonSceneLoadItem<WelcomeScreenController>, CustomUpdater {

        public HandControllerWrapper handControllerPrefab;
        public Transform handMount;
        public PuzSceneType overrideScene;
        public bool isNextSceneOverridden;
        public bool isResetPlayCount;
        
        public PuzButton chooseButton;
        public Animator welcomeAnim;
        public string welcomeState = "welcome";
        public string welcomeDoneState = "idle";
        public AudioClip bgm;
        private PuzButton exitButton = PuzButton.Exit;


        private enum StateFlow {
            NotEvenLoaded, Loading, Welcoming, PendingChoice, Chosen, PendingUnload, Unloading
        }

        private StateFlow currentState = StateFlow.NotEvenLoaded;
        private GlobalCameraController cameraController;
        private AudioController audioController;
        private const string PLAY_COUNT_PERF = "playCount";


        public override void Load() {
            cameraController = GlobalCameraController.GetSceneLoadInstance();
            cameraController.HideSkyBox();
            audioController = AudioController.GetSceneInstance();
            audioController.SetBgm(bgm);
            audioController.isBgmPlaying = true;

            GameObject handControllerInstance = Instantiate(handControllerPrefab.gameObject);
            TransformUtility.ChildAndNormalize(handMount, handControllerInstance.transform);

            HudController.GetSceneInstance().ShowHudItem(gameObject, Vector2.zero);
            currentState = StateFlow.Loading;
            GlobalCameraController.GetSceneLoadInstance().OnSceneFadeInFinish += HandleFadeFinish;
            SceneCustomDelegator.AddUpdater(this);
        }

        private void HandleFadeFinish(GlobalCameraController obj) {
            currentState++;
            obj.OnSceneFadeInFinish -= HandleFadeFinish;
            welcomeAnim.Play(welcomeState);
            SceneLoadSequencer.GetSceneInstance().OnUnloadFinished += HandleUnloaded;
        }

        private void HandleUnloaded(SceneLoadSequencer arg1) {
            arg1.OnUnloadFinished -= HandleUnloaded;
            cameraController.ShowSkyBox();
        }


        public void DoUpdate() {
            switch (currentState) {
                case StateFlow.Welcoming:
                    if (welcomeAnim.GetCurrentAnimatorStateInfo(0).IsName(welcomeDoneState)) {
                        StartPendingControllerSelection();
                    }
                break;

                case StateFlow.Chosen:
                    HandleSceneWrapUp();
                break;
            }
            
        }

        private void HandleSceneWrapUp() {
            audioController.isBgmPlaying = false;
            PuzSceneType nextScene = GetNextScene();
            IncrementPlayCount();
            if (isResetPlayCount) {
                ResetPlayCount();
            }
            currentState = StateFlow.PendingUnload;
            SceneNavigationController.GetSceneLoadInstance().RequestSceneTransition(nextScene);
        }

        private PuzSceneType GetNextScene() {
            PuzSceneType nextScene;
            if (isNextSceneOverridden) {
                nextScene = overrideScene;
            }
            else {
                int playCount = GetPlayCount();
                if (playCount == 0) {
                    nextScene = PuzSceneType.Tutorial;
                }
                else {
                    nextScene = PuzSceneType.MenuScreen;
                }
            }
            return nextScene;
        }

        private static void ResetPlayCount() {
            PlayerPrefs.SetInt(PLAY_COUNT_PERF, 0);
        }

        private static int GetPlayCount() {
            return PlayerPrefs.GetInt(PLAY_COUNT_PERF, 0);
        }

        private static void IncrementPlayCount() {
            int playCount = GetPlayCount();
            playCount++;
            PlayerPrefs.SetInt(PLAY_COUNT_PERF, playCount);
        }

        private void StartPendingControllerSelection() {
            currentState = StateFlow.PendingChoice;
            PuzButtonController.AddListener(chooseButton, PuzButtonDriverType.ShortPress, HandleControlTypeChosen);
            PuzButtonController.AddListener(exitButton, PuzButtonDriverType.LongPress, HandleExitPress);
        }

        private void HandleExitPress(PuzButtonEventData eventdata) {
            if (eventdata.eventType == PuzButtonEventType.PressConfirmed) {
                Debug.Log("Exit Requested");
                Application.Quit();
            }
        }

        private void HandleControlTypeChosen(PuzButtonEventData eventdata) {
            PuzButtonController.RemoveListener(chooseButton, PuzButtonDriverType.ShortPress, HandleControlTypeChosen);
            currentState = StateFlow.Chosen;
        }

        public override void Unload() {
            PuzButtonController.RemoveListener(exitButton, PuzButtonDriverType.LongPress, HandleExitPress);
            SceneCustomDelegator.RemoveUpater(this);
            HudController.GetSceneInstance().RemoveHudItem(gameObject);
            currentState = StateFlow.Unloading;
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
