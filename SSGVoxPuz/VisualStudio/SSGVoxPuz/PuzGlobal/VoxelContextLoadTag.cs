using SSGCore.Utility;
using SSGVoxel.Core;
using SSGVoxel.Internal;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    class VoxelContextLoadTag : PuzLoadTag{
        
        public override void HandleExtendedLoad(GameObject loadedObject) {
            VoxelContextCompState requestedState = loadedObject.GetComponent<VoxelContextCompState>();
            VoxelContextCompStateManager.SetContextToState(requestedState);
            TransformUtility.ChildAndNormalize(transform, VoxelContext.HostedObject.transform);
        }
    }
}
