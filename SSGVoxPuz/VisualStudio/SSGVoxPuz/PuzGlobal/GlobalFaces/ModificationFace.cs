using System;
using SSGVoxel.APIS;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface ModificationFace {
        event Action<VoxelWorldPosition, VoxelBlockType, VoxelBlockType> OnModification;
        void HaultModification();
    }
}
