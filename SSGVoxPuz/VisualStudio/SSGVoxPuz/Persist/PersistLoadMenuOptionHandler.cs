using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    public class PersistLoadMenuOptionHandler : PuzMenuOptionHandlerComp {

        public string textOptionId;
        private PuzTypeTextMenuOptionComp textMenuOption;

        public override void HandleOptionSelect() {
            PersistanceController.GetSceneLoadInstance().LoadWorld(textMenuOption.GetText());
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            textMenuOption = lookUp.FindMenuOption<PuzTypeTextMenuOptionComp>(textOptionId);
        }
    }
}
