using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    class PersistDeleteMenuOptionHandler : PuzMenuOptionHandlerComp {
        public string previewId = "preview-option";
        public PuzMenuScreenComp confirmScreen;
        public string deleteOptionId = "delete-confirmed";
        
        private PersistPreviewOptionHandler previewHandler;

        public override void HandleOptionSelect() {
            if (previewHandler.IsValid()) {
                DeleteMenuConfirmedMenuOption confirmOption = confirmScreen.FindMenuOption<DeleteMenuConfirmedMenuOption>(deleteOptionId);
                confirmOption.DeleteFileName = previewHandler.GetPreviewedModelName();
                PuzMenuControllerComp.GetSceneLoadInstance().NavToMenu(confirmScreen);
            }
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            previewHandler = lookUp.FindMenuOption<PersistPreviewOptionHandler>(previewId);
        }
    }
}
