using SSGVoxel.APIS;
using SSGVoxel.BlockBounds;
using SSGVoxel.Core;
using SSGVoxPuz.PuzMod;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    public class PuzToolHandlerPencil : PuzToolHandler {
        
        private readonly PuzModificationController modifyer;
        
        public PuzToolHandlerPencil(PuzModificationController modController) {
            modifyer = modController;
        }

        public bool HandleToolRequested(BoundsHit hit, ushort requested) {
            VoxelWorldPosition position = hit.AdjacentWorldPosition;
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
            return false;
        }
    }
}
