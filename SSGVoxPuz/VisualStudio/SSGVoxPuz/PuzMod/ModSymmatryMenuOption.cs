using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.PuzMod {
    public class ModSymmatryMenuOption : PuzToggleMenuOptionHandler {

        private PuzModificationController modController;


        protected override void HandleOptionSelect(bool isToggled) {
            modController.IsSymmatryEnabled = isToggled;
        }

        protected override void HandleOptionInitExtended(GameObject icon, PuzMenuOptionLookup aLookUp) {
            modController = PuzModificationController.GetSceneLoadInstance();
            modController.OnSymmatryModeChange += HandleSymModeChange;
        }

        private void HandleSymModeChange(PuzModificationController obj) {
            IsToggled = obj.IsSymmatryEnabled;
        }
    }
}
