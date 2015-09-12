using System;
using System.Collections.Generic;
using System.Linq;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxPuz.Environment;
using SSGVoxPuz.LeapAdapt;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    public class PuzController : SceneSingletonSceneLoadItem<PuzController>, CustomUpdater {
        public string voxelEditingLayer;
        public VoxelWorldComp puzWorldPrefab;
        public HandControllerWrapper handControllerPrefab;
        public Transform puzParent;
        public Transform handControllerParent;
        public ushort defaultBlockCode;
        public Transform cameraParent;
        public List<QuickLoadItem> toLoad;

        private bool isLoaded;
        private VoxelWorldComp puz;
        private PuzzleFacesController puzzleFacesController;
        private HandControllerWrapper handController;

        public event Action<PuzController> OnReset;

        public void Reset(TextAsset resetState) {
            Clear();
            if (OnReset != null) {
                OnReset(this);
            }
            puzzleFacesController.Persist.LoadWorld(resetState);
        }
        
        public void Reset() {
            Clear();
            if (OnReset != null) {
                OnReset(this);
            }
        }

        public void Clear() {
            Dictionary<VoxelWorldPosition, VoxelBlockType> allBlocks = puz.World.GetAllBlocks();
            List<VoxelWorldPosition> positions = allBlocks.Keys.ToList();
            bool hasZero = positions.Contains(VoxelWorldPosition.ZERO);
            if (!hasZero) {
                puz.SetBlock(VoxelWorldPosition.ZERO, VoxelBlockRegistry.GetBlockType(defaultBlockCode));
            }
            VoxelBlockType air = VoxelBlockRegistry.GetAirType();
            for (int i = 0; i < positions.Count; i++) {
                if (positions[i] != VoxelWorldPosition.ZERO) {
                    puz.SetBlock(positions[i], air);
                }
                else {
                    puz.SetBlock(VoxelWorldPosition.ZERO, VoxelBlockRegistry.GetBlockType(defaultBlockCode));
                }
            }
        }

        public void DoUpdate() {
            if (!IsLoaded) {
                VoxelContext.VoxelEditLayerName = voxelEditingLayer;
                isLoaded = true;
                Clear();
            }
        }

        public override bool IsLoaded { get { return isLoaded; } }

        public override bool IsUnloaded {
            get { return true; }
        }

        public override void Load() {
            puzzleFacesController = new PuzzleFacesController();
            GameObject puzWorldObject = Instantiate(puzWorldPrefab.gameObject);
            puz = puzWorldObject.GetComponentInChildren<VoxelWorldComp>();
            TransformUtility.ChildAndNormalize(puzParent, puzWorldObject.transform);

            GameObject handControllerObject = Instantiate(handControllerPrefab.gameObject);
            handController = handControllerObject.GetComponent<HandControllerWrapper>();
            TransformUtility.ChildAndNormalize(handControllerParent, handControllerObject.transform);
            handController.SetHandParent(handControllerParent);

            SceneCustomDelegator.AddUpdater(this);
            for (int i = 0; i < toLoad.Count; i++) {
                toLoad[i].Load();
            }
            SceneLoadSequencer.GetSceneInstance().OnLoadFinished += HandleLoadFinished;
        }

        private void HandleLoadFinished(SceneLoadSequencer obj) {
            obj.OnLoadFinished -= HandleLoadFinished;
            GlobalCameraController.GetSceneLoadInstance().HookToSceneMountPoint(cameraParent);
            obj.OnUnloadStarted += HandleUnloadStarted;
        }

        private void HandleUnloadStarted(SceneLoadSequencer sequencer, SceneLoadSequencer.UnloadBlockerData blockerData) {
            sequencer.OnUnloadStarted -= HandleUnloadStarted;
            CameraFadeOutBlockerData cameraFadeBlocker = new CameraFadeOutBlockerData();
            blockerData.blockers.Push(cameraFadeBlocker);
            GlobalCameraController.GetSceneLoadInstance().SendToLoadScreen();
            cameraFadeBlocker.PollForFadeFinish();
        }

        public PuzzleFacesController Faces {
            get {  return puzzleFacesController; }
        }

        public override void Unload() {
            for (int i = toLoad.Count-1; i >= 0; i--) {
                toLoad[i].Unload();
            }
        }

        public override void OnNextItemLoading() {
            
        }

        public override void OnNexuItemUnloading() {
            
        }

        public VoxelWorldComp GetPuzzleWorld() {
            return puz;
        }

        public HandControllerWrapper GetHandController() {
            return handController;
        }


        
    }
}
