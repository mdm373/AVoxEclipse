using System.IO;
using SSGVoxPuz.PuzMenu;

namespace SSGVoxPuz.Persist {
    public class SaveOverwriteOptionHandler : PersistPreviewOptionHandler {

        public PuzMenuScreenComp confirmScreen;
        public string saveConfirmMenuOptionName = "save-confirmed";

        public override void HandleOptionSelected(string voxFileName) {
            SaveMenuConfirmedMenuOption saveMenuOption = confirmScreen.FindMenuOption<SaveMenuConfirmedMenuOption>(saveConfirmMenuOptionName);
            saveMenuOption.SaveFileName = GetPreviewedModelName();
            PuzMenuControllerComp.GetSceneLoadInstance().NavToMenu(confirmScreen);
        }

    }
}
