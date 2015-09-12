using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    
    [Serializable]
    public class PuzInputRecordingData {
        [HideInInspector] public List<PuzInputRecordingDataSample> samples = new List<PuzInputRecordingDataSample>();
    }
}
