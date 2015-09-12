using System.Collections;
using System.Collections.Generic;
using SSGCore.Custom;
using SSGVoxPuz.PuzAudio;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.SceneNavigation;
using SSGVoxPuz.PuzInput;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial {
    public class TutorialController : SceneSingletonSceneLoadItem<TutorialController>, CustomUpdater {

        private float delay =3;
        public AudioClip bgm;
        public PuzButton exitTutorialKey;
        public List<TutorialEventConfig> eventConfigs;
        public PuzSceneType exitScene = PuzSceneType.MenuScreen;
        public List<QuickLoadItem> toLoad;
        
        private List<TutorialEventController> eventControllers;
        private int activeTutorialIndex;
        private TutorialEventController activeEventController;


        public override void Load() {
            PuzButtonController.AddListener(exitTutorialKey, PuzButtonDriverType.ShortPress, HandleSkipPressed);
            PuzButtonController.AddListener(exitTutorialKey, PuzButtonDriverType.LongPress, HandleExitPressed);
            activeTutorialIndex = 0;
            SceneCustomDelegator.AddUpdater(this);
            PopulateEventControllers();
            for (int i = 0; i < toLoad.Count; i++) {
                toLoad[i].Load();
            }
            SceneLoadSequencer.GetSceneInstance().OnLoadFinished += HandleLoadFinished;
        }

        private void HandleLoadFinished(SceneLoadSequencer obj) {
            AudioController.GetSceneInstance().SetBgm(bgm);
            AudioController.GetSceneInstance().isBgmPlaying = true;
            obj.OnLoadFinished -= HandleLoadFinished;
            StartTutorial();
        }

        private void HandleExitPressed(PuzButtonEventData eventdata) {
            if (eventdata.eventType == PuzButtonEventType.PressConfirmed) {
                ExitTutorial();
            }
        }

        public override void Unload() {
            if (activeEventController != null) {
                activeEventController.HandleEnded();
            }
            SceneCustomDelegator.RemoveUpater(this);
            PuzButtonController.RemoveListener(exitTutorialKey, PuzButtonDriverType.ShortPress, HandleSkipPressed);
            PuzButtonController.RemoveListener(exitTutorialKey, PuzButtonDriverType.LongPress, HandleExitPressed);
            for (int i = toLoad.Count -1; i >= 0; i--) {
                toLoad[i].Unload();
            }
        }

        private void HandleSkipPressed(PuzButtonEventData eventdata) {
            if (eventdata.eventType == PuzButtonEventType.PressConfirmed) {
                if (activeEventController != null) {
                    activeEventController.HandleSkipPressed();
                }
            }
        }

        private void PopulateEventControllers() {
            eventControllers = new List<TutorialEventController>();
            for (int i = 0; i < eventConfigs.Count; i++) {
                if (eventConfigs[i].isEnabled) {
                    eventControllers.Add(new TutorialEventController(eventConfigs[i]));
                }
            }
        }

        private void StartTutorial() {
            StartCoroutine(DelayedStart());
        }

        private IEnumerator<YieldInstruction> DelayedStart() {
            yield return new WaitForSeconds(delay);
            ActiveNextController();
        }

        private void ActiveNextController() {
            if (activeEventController != null) {
                activeEventController.HandleEnded();
            }
            if (activeTutorialIndex < eventControllers.Count) {
                activeEventController = eventControllers[activeTutorialIndex];
                activeEventController.HandleStarted();
                activeEventController.DoUpdate();
                activeTutorialIndex++;
            }
            else {
                activeEventController = null;
                ExitTutorial();
            }
        }

        private void ExitTutorial() {
            AudioController.GetSceneInstance().isBgmPlaying = false;
            SceneNavigationController.GetSceneLoadInstance().RequestSceneTransition(exitScene);
        }

        public void DoUpdate() {
            if (activeEventController != null) {
                if (activeEventController.IsBusy()) {
                    activeEventController.DoUpdate();
                }
                if(activeEventController != null && !activeEventController.IsBusy()) {
                    ActiveNextController();
                    DoUpdate();
                }
            }
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
