using SSGVoxPuz.PuzMenu;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.MainMenu {
    class LabelOptionHandler : PuzMenuOptionHandlerComp {

        public string label;
        private Text text;

        public override void HandleOptionSelect() {
            
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            text = icon.GetComponentInChildren<Text>();
            text.text = label;
        }
    }
}
