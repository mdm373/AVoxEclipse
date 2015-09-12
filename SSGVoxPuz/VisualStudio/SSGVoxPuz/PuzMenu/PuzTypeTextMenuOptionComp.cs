using SSGVoxPuz.PuzGlobal.PuzFont;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzMenu {
    public class PuzTypeTextMenuOptionComp : PuzMenuOptionHandlerComp {

        public string defaultText;
        private InputField input;
        public PuzFontType fontType = PuzFontType.Standard;

        public override void HandleOptionSelect() {
            
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup option) {
            input = icon.GetComponentInChildren<InputField>();
            input.textComponent.font = PuzFontController.GetSceneLoadInstance().GetFont(fontType);
        }

        public string GetText() {
            return input.text;
        }

        public void SetText(string text) {
            input.text = text;
        }
    }
}
