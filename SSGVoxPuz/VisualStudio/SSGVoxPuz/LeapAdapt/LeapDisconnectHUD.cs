using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.SceneNavigation;
using UnityEngine;

namespace SSGVoxPuz.LeapAdapt {
    class LeapDisconnectHud : QuickLoadItem{
        public Vector2 hudOffset;
        private SceneLoadSequencer sceneSequencer;
        private HudController hudController;
        private Transform originalParent;

        public override void Load() {
            originalParent = transform.parent;
            SceneNavigationController sceneNavigator = SceneNavigationController.GetSceneLoadInstance();
            sceneNavigator.OnSceneChanged += HandleSceneChanged;
            ListentToLoadFinish();
        }

        private void ListentToLoadFinish() {
            sceneSequencer = SceneLoadSequencer.GetSceneInstance();
            sceneSequencer.OnLoadFinished -= HandleLoadFinished;
            sceneSequencer.OnLoadFinished += HandleLoadFinished;
        }

        private void HandleSceneChanged(SceneNavigationController obj) {
            ListentToLoadFinish();
        }

        private void HandleLoadFinished(SceneLoadSequencer obj) {
            hudController = HudController.GetSceneInstance();
            hudController.ShowHudItem(gameObject, hudOffset);
            obj.OnLoadFinished -= HandleLoadFinished;
            obj.OnUnloadStarted += HandleUnloadStarted;
        }

        private void HandleUnloadStarted(SceneLoadSequencer arg1, SceneLoadSequencer.UnloadBlockerData arg2) {
            arg1.OnUnloadStarted -= HandleUnloadStarted;
            hudController.RemoveHudItem(gameObject);
            transform.parent = originalParent;
        }

        public override void Unload() {
            
        }
    }
}
