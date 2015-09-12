using SSGVoxel.APIS;
using SSGVoxel.BlockBounds;
using SSGVoxel.Core;
using SSGVoxPuz.PuzMod;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    public class PuzToolHandlerEraser : PuzToolHandler {
        
        private readonly PuzModificationController modifyer;

        public PuzToolHandlerEraser(PuzModificationController modController) {
            modifyer = modController;
        }

        public bool HandleToolRequested(BoundsHit hit, ushort requested) {
            VoxelBlockType airType = VoxelBlockRegistry.GetAirType();
            modifyer.SetBlock(hit.WorldPosition, airType.ByteCode);
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
