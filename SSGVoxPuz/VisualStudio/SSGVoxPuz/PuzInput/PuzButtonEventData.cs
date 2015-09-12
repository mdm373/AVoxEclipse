using System;

namespace SSGVoxPuz.PuzInput {
    [Serializable]
    public class PuzButtonEventData {
        public PuzButton button;
        public float axisDirection;
        public PuzButtonEventType eventType;
        public PuzButtonDriverType driverType;
        public bool isActualKeyStroke;
        public bool isConsumed;
    }
}
