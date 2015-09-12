using System.Collections.Generic;
using System.Linq;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.BlockSelection {
    class BlockSelectionMenuOption : PuzMenuOptionHandlerComp {

        [SerializeField] private ushort blockCode;

        public ushort BlockCode { get { return blockCode; } set { blockCode = value; } }

        public override void HandleOptionSelect() {
            BlockModelingController.GetSceneLoadInstance().ActiveBlockType =blockCode;
            PuzMenuControllerComp.GetSceneLoadInstance().HideMenu();
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            VoxelWorldComp worldComp = icon.GetComponentInChildren<VoxelWorldComp>();
            VoxelBlockVolume world = worldComp.World;
            Dictionary<VoxelWorldPosition, VoxelBlockType> blocks =world.GetAllBlocks();
            List<VoxelWorldPosition> positions = blocks.Keys.ToList();
            for (int i = 0; i < positions.Count; i++) {
                if (positions[i] != VoxelWorldPosition.ZERO) {
                    world.SetBlockType(positions[i], VoxelBlockRegistry.GetAirType());
                }
            }
            
            ModelingUtil.FillWithCode(icon, blockCode);
        }
    }
}
