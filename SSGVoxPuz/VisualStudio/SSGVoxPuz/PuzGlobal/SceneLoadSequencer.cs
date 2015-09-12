using System;
using System.Collections.Generic;
using System.Linq;
using SSGCore.Custom;
using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {

    public class SceneLoadSequencer : SceneSingletonBehaviour<SceneLoadSequencer>, CustomUpdater {

        public class UnloadBlockerData {
            public Stack<UnloadBlocker> blockers = new Stack<UnloadBlocker>();
        }

        public class UnloadBlocker {
            public bool IsBlocked { get; set; }
        }

        private enum LoadState {
            Idle, Loading, Loaded, Unloading, Unloaded
        }
        public GameObject sceneRoot;
        public event Action<SceneLoadSequencer> OnLoadFinished;
        public event Action<SceneLoadSequencer> OnUnloadFinished;
        public event Action<SceneLoadSequencer, UnloadBlockerData> OnUnloadStarted;

        public List<SceneLoadItem> loadSequence;
        private int loadIndex;
        private SceneLoadItem itemLoading;
        private LoadState currentLoadState;


        public void OnEnable() {
            if (currentLoadState == LoadState.Idle) {
                SceneCustomDelegator.AddUpdater(this);
                currentLoadState = LoadState.Loading;
                LoadNextItem();
            }
        }

        private void LoadNextItem() {
            if (itemLoading != null) {
                itemLoading.OnNextItemLoading();
            }
            if (loadIndex < loadSequence.Count) {
                itemLoading = loadSequence[loadIndex];
                itemLoading.Load();
                loadIndex++;
            }
            else {
                currentLoadState = LoadState.Loaded;
                itemLoading = null;
                if (OnLoadFinished != null) {
                    OnLoadFinished(this);
                }
            }
        }

        private void UnloadNextItem() {
            if (itemLoading != null) {
                itemLoading.OnNexuItemUnloading();
            }
            if (loadIndex >= 0) {
                itemLoading = loadSequence[loadIndex];
                itemLoading.Unload();
                Debug.Log("Unloading: " + itemLoading.name);
                loadIndex--;
            }
            else {
                Debug.Log("All Items Unloaded");
                currentLoadState = LoadState.Unloaded;
                itemLoading = null;
                DestroyUtility.DestroyAsNeeded(sceneRoot);
                if (OnUnloadFinished != null) {
                    OnUnloadFinished(this);
                }
            }
        }

        public void DoUpdate() {
            switch (currentLoadState) {
            case LoadState.Loading:
                if (itemLoading.IsLoaded) {
                    LoadNextItem();
                }
                break;
            case LoadState.Unloading:
                if (itemLoading.IsUnloaded) {
                    itemLoading.OnNexuItemUnloading();
                    UnloadNextItem();
                }
                break;
            }
            
        }

        public void HandleSceneExit() {
            UnloadBlockerData blockerData = new UnloadBlockerData();
            if (OnUnloadStarted != null) {
                OnUnloadStarted(this, blockerData);
            }
            StartCoroutine(UnloadBlockerCoroutine(blockerData));
        }

        private IEnumerator<YieldInstruction> UnloadBlockerCoroutine(UnloadBlockerData blockerData) {
            while (blockerData.blockers.Any()) {
                if (!blockerData.blockers.Peek().IsBlocked) {
                    blockerData.blockers.Pop();
                }
                yield return  new WaitForEndOfFrame();
            }
            Debug.Log("Unload Blockers Are Cleared");
            currentLoadState = LoadState.Unloading;
            loadIndex = loadSequence.Count -1;
            UnloadNextItem();
        }
    }
}
