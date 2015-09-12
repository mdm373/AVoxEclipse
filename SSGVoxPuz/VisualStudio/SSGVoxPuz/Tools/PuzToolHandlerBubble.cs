using System.Collections.Generic;
using SSGCore.Custom;
using SSGVoxel.APIS;
using SSGVoxel.BlockBounds;
using SSGVoxel.Core;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzMod;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    class PuzToolHandlerBubble : PuzToolHandler {
        
        private readonly DragIndicationController dragController;
        private readonly VoxelWorldComp previewWorldComp;
        private readonly PuzModificationController modifyer;
        private readonly  AudioClip confirmSound;
        private Transform activeGesture;
        private ushort requestedBlockType;
        private BoundsHit requestedHit;
        private int oldValue;
        
        private List<VoxelWorldPosition> requestedPositions = new List<VoxelWorldPosition>();
        private bool wasBubbleConfirmed;


        public PuzToolHandlerBubble(CustomBehaviour parent, PuzToolHandlerBubbleConfig config, VoxelWorldComp previewWorld, PuzModificationController modController) {
            dragController = new DragIndicationController {Config = config.dragConfig};
            dragController.OnLoad(parent);
            previewWorldComp = previewWorld;
            confirmSound = config.confirmSound;
            modifyer = modController;
        }

        public bool HandleToolRequested(BoundsHit hit, ushort requested) {
            bool isConfirmed = false;
            if (!dragController.IsDragging && !wasBubbleConfirmed) {
                dragController.StartDragging(activeGesture, activeGesture.position);
                requestedBlockType = requested;
                requestedHit = hit;
                previewWorldComp.GameObject.SetActive(true);
                previewWorldComp.GameObject.transform.position = VoxelSpaceUtil.GetWorldSpacePosition(hit.World,
                    hit.AdjacentWorldPosition);
                previewWorldComp.transform.rotation = hit.World.GetTransform().rotation;
                oldValue = 0;
                HandleValueChanged(oldValue);
                isConfirmed = true;
            }
            else {
                wasBubbleConfirmed = false;
            }
            return isConfirmed;
        }

        public void SetGesture(Transform gesture) {
            activeGesture = gesture;
            dragController.StopDragging();
            
        }

        public void HandleSelectionCanceled() {
            dragController.StopDragging();
            previewWorldComp.GameObject.SetActive(false);
        }

        public void DoUpdate() {
            dragController.DoUpdate();
            if (dragController.IsDragging) {
                int currentValue = (int) dragController.CurrentValue;
                if (currentValue != oldValue) {
                    oldValue = currentValue;
                    HandleValueChanged(currentValue);
                }
            }
        }

        private void HandleValueChanged(int value) {
            VoxelBlockVolume previewWorld = previewWorldComp.World;
            List<VoxelWorldPosition> priorRequestedPositions = requestedPositions;
            requestedPositions = new List<VoxelWorldPosition>();

            float valueTolerance = .5f + value;
            VoxelBlockType blockType = VoxelBlockRegistry.GetBlockType(requestedBlockType);
            for (int x = -value; x <= value; x++) {
                for (int y = -value; y <= value; y++) {
                    for (int z = -value; z <= value; z++) {
                        Vector3 currentVec = new Vector3(x, y, z);
                        float magnitudeSq = currentVec.sqrMagnitude;
                        if (magnitudeSq <= (valueTolerance * valueTolerance)) {
                            VoxelWorldPosition requestedPosition = new VoxelWorldPosition(currentVec);
                            requestedPositions.Add(requestedPosition);
                            if (priorRequestedPositions.Contains(requestedPosition)) {
                                priorRequestedPositions.Remove(requestedPosition);
                            }
                            else {
                                previewWorld.SetBlockType(requestedPosition, blockType);
                            }
                        }
                    }
                }
            }

            VoxelBlockType airType = VoxelBlockRegistry.GetAirType();
            for (int i = 0; i < priorRequestedPositions.Count; i++) {
                previewWorld.SetBlockType(priorRequestedPositions[i], airType);
            }
        }

        public void HandlePress() {
            if (dragController.IsDragging) {
                for (int i = 0; i < requestedPositions.Count; i++) {
                    VoxelWorldPosition actual = requestedHit.AdjacentWorldPosition + requestedPositions[i];
                    modifyer.SetBlock(actual, requestedBlockType);
                }
                HandleSelectionCanceled();
                AudioSource.PlayClipAtPoint(confirmSound, activeGesture.position);
                wasBubbleConfirmed = true;
            }
            else {
                wasBubbleConfirmed = false;
            }
        }

        public bool IsEditWithPendingModificationAllowed() {
            return false;
        }
    }
}
