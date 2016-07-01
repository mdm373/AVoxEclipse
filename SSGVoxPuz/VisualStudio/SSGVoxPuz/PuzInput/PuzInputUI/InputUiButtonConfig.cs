using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSGVoxPuz.PuzInput.PuzInputUI {
    [Serializable]
    public class InputUiButtonConfig {
        public string name = "Keyboard-A"; //UnityName
        public string descriptiveText = "the 'A' key";
        public InputUiButtonType type = InputUiButtonType.KeyboardSmall;
        public string displayText = "Key";
    }
}
