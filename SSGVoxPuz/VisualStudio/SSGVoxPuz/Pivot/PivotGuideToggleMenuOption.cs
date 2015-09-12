using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Pivot {
    public class PivotGuideToggleMenuOption : PuzToggleMenuOptionHandler {
        private PivotGuidesControllerComp guideController;


        protected override void HandleOptionInitExtended(GameObject icon, PuzMenuOptionLookup aLookUp) {
            guideController = PivotGuidesControllerComp.GetSceneLoadInstance();
            guideController.OnGuideShowChange += HandleGuideShowChange;
        }
        
        protected override void HandleOptionSelect(bool isToggled) {
            guideController.SetGuidesShown(!isToggled);
        }

        private void HandleGuideShowChange(PivotGuidesControllerComp aGuideController) {
            SetToggleState(!guideController.IsGuidesShown());
        }
    }
}
