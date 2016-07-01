using System;
using UnityEngine;

namespace SSGVoxPuz.PuzInput {

    [Serializable]
    public enum KeyType {
        Keyboard,Mouse,XInput
    }

    [Serializable]
    public enum MouseButton {
        Left, ScrollUp, ScrollDown, ScrollClick, Right
    }

    [Serializable]
    public class PuzButtonConfigEntry {
        public string name = "Button Display Name";
        public PuzButton button;
        public bool isAlwaysListenedTo;
        public KeyType keyType;
        public KeyCode keyCode;
        public MouseButton mouseButton;
        public XInputKeyCode xInputKeyCode;

        public string GetComparisonKey() {
            return keyType + " " + keyCode + " " + mouseButton + " " + xInputKeyCode;
        }
    }
}
