using SSGCore.Custom;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzInput.PuzInputUI {

    public class InputUiIcon : CustomBehaviour{
        public InputUiButtonType buttonType;
        public Text text;
        public string buttonAnim = "Wiggle";
        public string axisAnim = "Wave";
        
        public bool IsAxis { get; set; }
    }
}
