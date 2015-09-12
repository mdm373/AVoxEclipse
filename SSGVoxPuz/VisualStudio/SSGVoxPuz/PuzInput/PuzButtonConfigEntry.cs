using System;

namespace SSGVoxPuz.PuzInput {
    [Serializable]
    public class PuzButtonConfigEntry {
        public string name = "Button Display Name";
        public PuzButton button;
        public string unityInputName;
        public bool isAxis;
        public bool isUnityInputAxis;
        public bool isForPositiveUnityAxis;
        public bool isAlwaysListenedTo;
        public bool isAxisInverted;
    }
}
