using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.SceneNavigation {
    public class ExitGameMenuOption : PuzMenuOptionHandlerComp{
        private SceneNavigationController navigator;

        public override void HandleOptionSelect() {
            navigator.RequestGameExit();
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            navigator = SceneNavigationController.GetSceneLoadInstance();
        }
    }
}
 