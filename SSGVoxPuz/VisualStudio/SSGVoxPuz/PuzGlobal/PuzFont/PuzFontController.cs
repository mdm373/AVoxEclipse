using System.Collections.Generic;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.PuzFont {
    public class PuzFontController : SceneSingletonQuickLoadItem<PuzFontController> {

        public List<PuzFontConfig> config;
        private Dictionary<PuzFontType, Font> fontMap;

        public override void Load() {
            fontMap = new Dictionary<PuzFontType, Font>();
            for (int i = 0; i < config.Count; i++) {
                fontMap[config[i].type] = config[i].font;
            }
        }

        public override void Unload() {
            fontMap = null;
        }

        public Font GetFont(PuzFontType type) {
            return fontMap[type];
        }

    }
}
