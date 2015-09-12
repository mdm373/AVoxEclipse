using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using SSGVoxel.APIS;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface SelectionFace {
        void EnableSelection();
        void DisableSelection();
        event Action<SelectionFace> OnSelectionChange;
        VoxelWorldPosition CurrentSelectionPosition { get; }
        VoxelBlockType CurrentSelectionType { get; }

    }
}
