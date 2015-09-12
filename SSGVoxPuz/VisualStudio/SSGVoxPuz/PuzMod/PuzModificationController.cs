using System;
using System.Collections.Generic;
using System.Linq;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxPuz.BlockSelection;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using UnityEngine;

namespace SSGVoxPuz.PuzMod {

    public class PuzModificationController : SceneSingletonQuickLoadItem<PuzModificationController>, CustomUpdater, ModificationFace {

        public event Action<VoxelWorldPosition, VoxelBlockType, VoxelBlockType> OnModification;
        
        public event Action<PuzModificationController> OnSymmatryModeChange;

        public uint maxConcurrent = 1;
        public string contextName;
        public Transform startTransform;
        public float staggerTime;
        public ModHandlerConfig modConfig;

        private readonly Queue<ModControllerRequest> requestQueue = new Queue<ModControllerRequest>();
        private readonly Queue<ModRequestHandler> idleRequestHandlers = new Queue<ModRequestHandler>();
        private readonly List<ModRequestHandler> workingRequestHandlers = new List<ModRequestHandler>();
        private readonly List<ModRequestHandler> stateChangedRequestHandlers = new List<ModRequestHandler>();
        private float lastUpdateTime = float.MinValue;

        private bool isSymmatryEnabled;
        private PuzController puzController;

        public void HaultModification() {
            requestQueue.Clear();
            for (int i = 0; i < workingRequestHandlers.Count; i++) {
                workingRequestHandlers[i].Hault(false);
                idleRequestHandlers.Enqueue(workingRequestHandlers[i]);
            }
            workingRequestHandlers.Clear();
        }

        public bool IsSymmatryEnabled {
            get {
                return isSymmatryEnabled;
            }
            set {
                bool oldValue = isSymmatryEnabled;
                isSymmatryEnabled = value; 
                if (oldValue != isSymmatryEnabled) {
                    if (OnSymmatryModeChange != null) {
                        OnSymmatryModeChange(this);
                    }
                }
                
            }
        }


        public override void Load() {
            SceneCustomDelegator.AddUpdater(this);
            puzController = PuzController.GetSceneLoadInstance();
            puzController.OnReset += HandleReset;
            puzController.Faces.Modification = this;
            Dictionary<ushort, VoxelSingleBlockWorldIndividualCache> caches = BuildCaches();
            for (int i = 0; i < maxConcurrent; i++) {
                ModRequestHandler handler = new ModRequestHandler(puzController.GetPuzzleWorld().World, caches, modConfig);
                idleRequestHandlers.Enqueue(handler);
            }
            
        }

        public override void Unload() {
            puzController.OnReset -= HandleReset;
        }

        private void HandleReset(PuzController obj) {
            IsSymmatryEnabled = false;
        }

        private Dictionary<ushort, VoxelSingleBlockWorldIndividualCache> BuildCaches() {
            BlockModelingController blockController = BlockModelingController.GetSceneLoadInstance();
            Dictionary<ushort, VoxelSingleBlockWorldIndividualCache> caches = new Dictionary<ushort, VoxelSingleBlockWorldIndividualCache>();
            List<ushort> modelingBlocks = blockController.GetModelingBlocks();
            for (int i = 0; i < modelingBlocks.Count; i++) {
                VoxelBlockType blockType = VoxelBlockRegistry.GetBlockType(modelingBlocks[i]);
                GameObject parentObject = new GameObject("block-" + blockType.ByteCode);
                TransformUtility.ChildAndNormalize(transform, parentObject.transform);
                VoxelSingleBlockWorldIndividualCache cache = new VoxelSingleBlockWorldIndividualCache(blockType,
                    contextName, parentObject.transform);
                caches[blockType.ByteCode] = cache;
                cache.CacheSize = maxConcurrent;
                cache.BuildCache();
            }
            return caches;
        }

        public void SetBlock(VoxelWorldPosition position, ushort code) {
            VoxelBlockType blockType = VoxelBlockRegistry.GetBlockType(code);
            VoxelBlockType existingBlockType = puzController.GetPuzzleWorld().World.GetBlockType(position);
            ModControllerRequest request = new ModControllerRequest {
                position = position,
                blockType = blockType,
                existingBlockType = existingBlockType
            };
            requestQueue.Enqueue(request);
            if (IsSymmatryEnabled) {
                VoxelWorldPosition symPosition = new VoxelWorldPosition(-position.X, position.Y, position.Z);
                ModControllerRequest symRequest = new ModControllerRequest {
                    position = symPosition,
                    blockType = blockType,
                    existingBlockType = existingBlockType
                };  
                requestQueue.Enqueue(symRequest);
            }

        }

        public void DoUpdate() {
            for (int i = 0; i < workingRequestHandlers.Count; i++) {
                if (workingRequestHandlers[i].IsBusy()) {
                    workingRequestHandlers[i].DoUpdate();
                }
                else {
                    stateChangedRequestHandlers.Add(workingRequestHandlers[i]);
                }
            }
            if (stateChangedRequestHandlers.Any()) {
                for (int i = 0; i < stateChangedRequestHandlers.Count; i++) {
                    workingRequestHandlers.Remove(stateChangedRequestHandlers[i]);
                    idleRequestHandlers.Enqueue(stateChangedRequestHandlers[i]);
                }
                stateChangedRequestHandlers.Clear();
            }

            float now = Time.time;
            bool isStaggered = (now - lastUpdateTime) < staggerTime;
            if (!isStaggered && requestQueue.Any() && idleRequestHandlers.Any()) {
                ModRequestHandler handler = idleRequestHandlers.Dequeue();
                workingRequestHandlers.Add(handler);
                lastUpdateTime = now;
                ModControllerRequest request = requestQueue.Dequeue();
                handler.StartHandling(request);
                if (OnModification != null) {
                    OnModification(request.position, request.existingBlockType, request.blockType);
                }
            }
            puzController.GetPuzzleWorld().OnRenderObject();
        }

        public int GetPendingModificationsCount() {
            return workingRequestHandlers.Count + requestQueue.Count;
        }

    }
}
