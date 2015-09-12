using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSGVoxPuz.PuzInput {
    [Serializable]
    public class PuzButtonsGeneralConfig {
        public float axisTolerance = .01f;
        public float shortToLongPressSeconds  = .75f;
    }
}
