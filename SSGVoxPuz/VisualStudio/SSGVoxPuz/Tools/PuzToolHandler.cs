using SSGVoxel.BlockBounds;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    public interface PuzToolHandler {

        bool HandleToolRequested(BoundsHit hit, ushort requested);
        void SetGesture(Transform gesture);
        void HandleSelectionCanceled();
        void DoUpdate();
        void HandlePress();
        bool IsEditWithPendingModificationAllowed();
    }
}
