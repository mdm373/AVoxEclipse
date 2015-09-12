using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxel.Internal;
using SSGVoxPuz.PuzGlobal;

namespace SSGVoxPuz.BlockSelection {
    class ModelBlockGenerator : SceneSingletonQuickLoadItem<ModelBlockGenerator> {

        public VoxelBlockMetaDataEntryIntegerState textureIndex;
        public VoxelBlockDetailsState modelDetails;
        public ushort baseIndex = 1000;
        public ushort textureCount = 64;
        private readonly string editLayerName = "vox-edit";

        public override void Load() {
            VoxelContext.VoxelEditLayerName = "Default";
            VoxelContext.VoxelEditLayerName = editLayerName;
            int texIndex = 0;
            int max = baseIndex + textureCount;
            for (ushort index = baseIndex; index < max; index++) {
                VoxelBlockType registeredInstance = VoxelBlockRegistry.GetBlockType(index);
                if (CompUtil.IsNull(registeredInstance)) {
                    modelDetails.byteCode = index;
                    modelDetails.blockName = "Model Block-" + texIndex;
                    textureIndex.Value = texIndex;
                    VoxelContextCompStateManager.AddBlockRegistration(modelDetails);
                    texIndex++;
                }

            }
        }

        public override void Unload() {
            
        }

    }
}
