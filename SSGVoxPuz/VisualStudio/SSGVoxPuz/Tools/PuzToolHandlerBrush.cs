using SSGVoxel.APIS;
using SSGVoxel.BlockBounds;
using SSGVoxel.Core;
using SSGVoxPuz.PuzMod;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    
    class PuzToolHandlerBrush : PuzToolHandler{
        
        private readonly PuzModificationController modifyer;

        public PuzToolHandlerBrush(PuzModificationController modController) {
            modifyer = modController;
        }
        
        public bool HandleToolRequested(BoundsHit hit, ushort requested) {
            VoxelWorldPosition position = hit.WorldPosition;
            modifyer.SetBlock(position, requested);
            return true;
        }

        public void SetGesture(Transform gesture) {
            
        }

        public void HandleSelectionCanceled() {
            
        }

        public void DoUpdate() {
            
        }

        public void HandlePress() {
            
        }

        public bool IsEditWithPendingModificationAllowed() {
            return true;
        }
    }
}
