using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    class PreviewIncrementOption : PuzMenuOptionHandlerComp{
        
        public string previewId = "preview-option";
        public bool isForward = true;

        private PersistPreviewOptionHandler previewOption;

        public override void HandleOptionSelect() {
            if (isForward) {
                previewOption.IncrementPreviewIndex();
            }
            else {
                previewOption.DecrementSaveIndex();
            }
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            previewOption = lookUp.FindMenuOption<PersistPreviewOptionHandler>(previewId);
        }
    }
}
