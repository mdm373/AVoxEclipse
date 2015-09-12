using System;
using System.Collections.Generic;
using SSGVoxPuz.PuzInput;

namespace SSGVoxPuz.PuzTutorial.Recording {
    [Serializable]
    public class PuzInputRecordingDataSample {

        public float deltaTime;
        public List<PuzButtonEventData> buttonEvents = new List<PuzButtonEventData>();

    }
}
