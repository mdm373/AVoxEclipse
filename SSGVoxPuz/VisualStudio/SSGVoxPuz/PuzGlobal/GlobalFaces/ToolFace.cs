using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.Tools;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface ToolFace {
        void DisableTools();
        void EnableTools();
        void SetTool(PuzTool tool);
    }
}
