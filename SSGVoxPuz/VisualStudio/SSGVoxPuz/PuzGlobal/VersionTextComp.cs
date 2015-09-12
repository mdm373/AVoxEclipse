using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal.PuzFont;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzGlobal {
    class VersionTextComp : QuickLoadItem {
        
        public Text text;
        public string format = "build: {0}";
        public PuzFontType fontType;

        public override void Load() {
            Font font = PuzFontController.GetSceneLoadInstance().GetFont(fontType);
            text.font = font;
            text.text = string.Format(format, Versioning.GetBuildLongVersion());
        }

        public override void Unload() {
            
        }
    }
}
