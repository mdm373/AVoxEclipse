using System;
using SSGVoxel.Core;
using SSGVoxelEditor.APIS;
using SSGVoxPuz.BlockSelection;

namespace SSGVoxPuzEditor.BlockSelection {
    class ModelingMetaDataEntryFactory : VoxelBlockMetaDataEntryFactory {
        
        protected override Type MetaDataEntryType {
            get { return typeof (VoxelBlockMetaDataEntryBoolean); }
        }

        protected override string Key {
            get { return ModelingConstants.META_DATA_KEY; }
        }

        protected override object DefaultValue {
            get { return false; }
        }
    }
}
