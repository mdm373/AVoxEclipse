using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface BlockTypeFace {

        void EnableBlockTypeChange();
        void DisableBlockTypeChange();
    }
}
