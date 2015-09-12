using System;
using System.Collections.Generic;

namespace SSGVoxPuz.PuzInput {
    
    [Serializable]
    public class PuzButtonGrouping {
        public string name = "Grouping Display Name";
        public bool isGamePadIncluded;
        public PuzInputHandedness handedness;
        public PuzButtonGroupingType groupingType;
        public List<PuzButtonConfigEntry> entries = new List<PuzButtonConfigEntry>();
        public int orderIndex = 0;
    }
}