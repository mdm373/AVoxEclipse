using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzGlobal.PuzFont {
    class PuzFontTag : QuickLoadItem {

        public Text text;
        public PuzFontType fontType;

        public override void Load() {
            text.font = PuzFontController.GetSceneLoadInstance().GetFont(fontType);
        }

        public override void Unload() {
            
        }
    }
}
