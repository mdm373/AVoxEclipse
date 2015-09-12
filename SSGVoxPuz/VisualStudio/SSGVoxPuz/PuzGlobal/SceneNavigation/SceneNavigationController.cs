using System;
using System.Collections.Generic;
using System.Linq;
using SSGCore.Custom;
using SSGVoxPuz.Environment;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.SceneNavigation {
    class SceneNavigationController : SceneSingletonQuickLoadItem<SceneNavigationController>, CustomUpdater {

        //Things Listining to this should also be walking scenes...
        public event Action<SceneNavigationController> OnSceneChanged;

        public List<SceneNavigationConfig> configs;

        private Dictionary<PuzSceneType, SceneNavigationConfig> configMap;
        private readonly List<CustomBehaviour>  transitionBlockers = new List<CustomBehaviour>();
        private SceneNavigationConfig matchConfig;
        private bool isNavigationEnabled;
        private PuzSceneType queuedTransition = PuzSceneType.Unknown;

        public void RequestSceneTransition(PuzSceneType sceneType) {
            if (isNavigationEnabled) {
                if (!transitionBlockers.Any()) {
                    isNavigationEnabled = false;
                    matchConfig = configMap[sceneType];
                    SceneLoadSequencer loadSequencer = SceneLoadSequencer.GetSceneInstance();
                    loadSequencer.OnUnloadFinished += HandleUnloadFinished;
                    loadSequencer.HandleSceneExit();
                }
                else {
                    queuedTransition = sceneType;
                }
            }
        }

        
        private void HandleUnloadFinished(SceneLoadSequencer obj) {
            obj.OnUnloadFinished -= HandleUnloadFinished;
            Application.LoadLevel(matchConfig.sceneName);
            matchConfig = null;

        }

        public void RequestGameExit() {
            Application.Quit();
        }
        public override void Load() {
            SceneCustomDelegator.AddUpdater(this);
            configMap = new Dictionary<PuzSceneType, SceneNavigationConfig>();
            for (int i = 0; i < configs.Count; i++) {
                configMap[configs[i].type] = configs[i];
            }
        }

        public override void Unload() {
            SceneCustomDelegator.RemoveUpater(this);
        }

        public void FlagSceneChanged() {
            isNavigationEnabled = true;
            if (OnSceneChanged != null) {
                OnSceneChanged(this);
            }
        }

        public void AddTransitionBlocker(CustomBehaviour blocker) {
            transitionBlockers.Add(blocker);
        }

        public void RemoveTransitionBlocker(CustomBehaviour blocker) {
            transitionBlockers.Remove(blocker);
        }

        public void DoUpdate() {
            if (queuedTransition != PuzSceneType.Unknown) {
                if (!transitionBlockers.Any()) {
                    PuzSceneType scene = queuedTransition;
                    queuedTransition = PuzSceneType.Unknown;
                    RequestSceneTransition(scene);
                }
            }
        }
    }
}
