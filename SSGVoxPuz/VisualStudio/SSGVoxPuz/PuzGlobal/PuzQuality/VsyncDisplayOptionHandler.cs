using System;
using SSGVoxPuz.PuzGlobal.PuzFont;
using SSGVoxPuz.PuzMenu;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzGlobal.PuzQuality {
    class VsyncDisplayOptionHandler : PuzMenuOptionHandlerComp {
        public PuzFontType fontType;
        public string format = "V-Sync Enabled: {0}";
        
        private Text text;

        public override void HandleOptionSelect() {
            
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            text = icon.GetComponentInChildren<Text>();
            text.font = PuzFontController.GetSceneLoadInstance().GetFont(fontType);
            PuzQualityManager qualityManager = PuzQualityManager.GetSceneLoadInstance();
            // ReSharper disable once DelegateSubtraction
            qualityManager.onVsyncChanged -= HandleVsyncChanged;
            qualityManager.onVsyncChanged += HandleVsyncChanged;
            HandleVsyncChanged(qualityManager);

        }

        private void HandleVsyncChanged(PuzQualityManager obj) {
            text.text = string.Format(format, obj.IsVsyncEnabled());
        }
    }
}
