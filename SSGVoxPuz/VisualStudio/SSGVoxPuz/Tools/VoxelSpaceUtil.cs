using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxel.APIS;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    class VoxelSpaceUtil {

        public static Vector3 GetWorldSpacePosition(VoxelBlockVolume world, VoxelWorldPosition position) {
            Vector3 worldPosition = world.GetTransform().TransformPoint(position.GetVector());
            return worldPosition;
        }
    }
}
