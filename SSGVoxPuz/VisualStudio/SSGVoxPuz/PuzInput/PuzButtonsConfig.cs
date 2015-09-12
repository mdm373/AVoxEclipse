using System;
using System.Collections.Generic;

namespace SSGVoxPuz.PuzInput {
    
    [Serializable]
    public class PuzButtonsConfig {
        public PuzButtonGroupingType defaultGrouping;
        public List<PuzButtonGrouping> groupings;
        public PuzButtonsGeneralConfig generalConfig;

    }
}
