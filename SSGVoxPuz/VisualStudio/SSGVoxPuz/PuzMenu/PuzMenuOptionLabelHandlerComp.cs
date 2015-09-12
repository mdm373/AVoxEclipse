using SSGVoxPuz.PuzGlobal.PuzFont;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzMenu {
    public class PuzMenuOptionLabelHandlerComp : PuzMenuOptionHandlerComp {

        public string displayText;
        public PuzFontType fontType = PuzFontType.Standard;
        private Text labelText;

        public override void HandleOptionSelect() {
            //Do Nothing
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            labelText = icon.GetComponentInChildren<Text>();
            labelText.text = displayText;
            labelText.font = PuzFontController.GetSceneLoadInstance().GetFont(fontType);
        }

        public void SetText(string text) {
            displayText = text;
            labelText.text = text;
        }

        public string GetText() {
            return displayText;
        }
    }
}
