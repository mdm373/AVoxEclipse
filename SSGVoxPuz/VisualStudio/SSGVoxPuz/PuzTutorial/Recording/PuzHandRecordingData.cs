using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    
    [Serializable]
    public class PuzHandRecordingData {
        public List<PuzHandRecordingDataSample> samples = new List<PuzHandRecordingDataSample>();
    }
}
