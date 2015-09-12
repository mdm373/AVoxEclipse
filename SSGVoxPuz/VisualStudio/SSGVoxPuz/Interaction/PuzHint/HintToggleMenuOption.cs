using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Interaction.PuzHint {
    class HintToggleMenuOption : PuzToggleMenuOptionHandler {
        private HintFace hintFace;

        protected override void HandleOptionSelect(bool isToggled) {
            if (isToggled) {
                hintFace.DisableHints();
            }
            else {
                hintFace.EnableHints();
            }
        }

        protected override void HandleOptionInitExtended(GameObject icon, PuzMenuOptionLookup aLookUp) {
            hintFace = PuzController.GetSceneLoadInstance().Faces.Hint;
        }
    }
}
