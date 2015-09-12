using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxel.Modifications;
using UnityEngine;

namespace SSGVoxPuz.BlockSelection {
    class ModelingUtil {

        private const int FILL_SCALE = 10;
        private static readonly VoxelWorldPosition FROM = new VoxelWorldPosition(-FILL_SCALE, -FILL_SCALE, -FILL_SCALE);
        private static readonly VoxelWorldPosition TO = new VoxelWorldPosition(FILL_SCALE, FILL_SCALE, FILL_SCALE);

        public static void FillWithCode(GameObject icon, ushort blockCode) {
            VoxelWorldComp worldComp = icon.GetComponentInChildren<VoxelWorldComp>();
            VoxelBlockType blockType = VoxelBlockRegistry.GetBlockType(blockCode);
            VoxelFillUtil.FillArea(worldComp.World, blockType, FROM, TO, VoxelFillUtil.VoxelFillMode.Existing);
        }

        public static void FillWithCode(VoxelBlockVolume icon, ushort blockCode) {
            VoxelBlockType blockType = VoxelBlockRegistry.GetBlockType(blockCode);
            VoxelFillUtil.FillArea(icon, blockType, FROM, TO, VoxelFillUtil.VoxelFillMode.Existing);
        }
    }
}
