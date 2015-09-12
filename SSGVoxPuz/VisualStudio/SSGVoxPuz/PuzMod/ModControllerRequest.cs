using SSGVoxel.APIS;

namespace SSGVoxPuz.PuzMod {
    struct ModControllerRequest {
        public VoxelBlockType blockType;
        public VoxelBlockType existingBlockType;
        public VoxelWorldPosition position;

    }
}
