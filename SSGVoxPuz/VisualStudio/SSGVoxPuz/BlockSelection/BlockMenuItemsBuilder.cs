using System.Collections.Generic;
using SSGVoxel.Core;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.BlockSelection {
    public class BlockMenuItemsBuilder : PuzDynaMenuBuilder {

        public Vector3 offset;
        public float height;
        public int cols = 1;
        public float topOffSet;

        public override void BuildDynamicItems(GameObject options, PuzScreenLayoutConfig layout) {
            BlockModelingController modelingController = BlockModelingController.GetSceneLoadInstance();
            List<ushort> modelingBlocks = modelingController.GetModelingBlocks();
            GameObject iconPrefab = modelingController.IconPrefab;

            for (int i = 0; i < modelingBlocks.Count; i++) {
                int col = i % cols;
                int row = i / cols;
                Vector3 position = offset;
                position = position*col;
                position.x = (layout.screenWidth/2.0f) - position.x ;
                position.y = (layout.screenHeight / 2.0f) - (row * height) - (topOffSet);
                BlockSelectionMenuOption selectionOption = options.AddComponent<BlockSelectionMenuOption>();
                selectionOption.IconPrefab = iconPrefab;
                selectionOption.HoverText = VoxelBlockRegistry.GetBlock(modelingBlocks[i]).BlockName;
                selectionOption.BlockCode = modelingBlocks[i];
                selectionOption.OptionType = PuzMenuOptionType.Small;
                layout.positions.Add(position);
            }
        }
    }
}
