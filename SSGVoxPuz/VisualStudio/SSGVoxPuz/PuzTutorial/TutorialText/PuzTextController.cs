using System.Collections.Generic;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialText {
    public class PuzTextController : SceneSingletonQuickLoadItem<PuzTextController> {

        public List<TextConfigEntry> configs;

        public GeneralTextBoxConfig generalConfig;
        private Dictionary<TextBoxType, TextBox> configMap;

        public override void Load() {
            configMap = new Dictionary<TextBoxType, TextBox>();
            for (int i = 0; i < configs.Count; i++) {
                configMap[configs[i].type] = configs[i].textBox;
            }
        }

        public override void Unload() {
            configMap = null;
        }

        public TextBox GetTextBox(TextBoxType type) {
            GameObject instance = null;
            instance = Instantiate(configMap[type].GameObject);
            instance.name = type + "scrolling-text-box";
            TextBox box = instance.GetComponent<TextBox>();
            box.GeneralConfig = generalConfig;
            return box;
        }

        public void ReturnTextBox(TextBox textBox, TextBoxType textBoxType) {
            DestroyUtility.DestroyAsNeeded(textBox.GameObject);
        }
    }
}
