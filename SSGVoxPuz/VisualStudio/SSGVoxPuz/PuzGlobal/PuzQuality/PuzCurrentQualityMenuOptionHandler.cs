using SSGVoxPuz.PuzGlobal.PuzFont;
using SSGVoxPuz.PuzMenu;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzGlobal.PuzQuality {
    class PuzCurrentQualityMenuOptionHandler : PuzMenuOptionHandlerComp {
        public PuzFontType fontType = PuzFontType.Standard;
        public string format = "Quality: {0}";
        private Text text;

        public override void HandleOptionSelect() {
            
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            text = icon.GetComponentInChildren<Text>();
            PuzQualityManager qualityManager = PuzQualityManager.GetSceneLoadInstance();
            // ReSharper disable once DelegateSubtraction
            qualityManager.onQualityLevelChange -= HandleQualityLevelChange;
            qualityManager.onQualityLevelChange += HandleQualityLevelChange;
            text.font = PuzFontController.GetSceneLoadInstance().GetFont(fontType);
            UpdateQualityDisplay(qualityManager);
        }

        private void UpdateQualityDisplay(PuzQualityManager qualityManager) {
            text.text = string.Format(format, qualityManager.GetCurrentQualityLevelDescription());
        }

        private void HandleQualityLevelChange(PuzQualityManager obj) {
            UpdateQualityDisplay(obj);
        }
    }
}
