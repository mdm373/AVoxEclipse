using System.IO;
using SSGVoxPuz.PuzMenu;

namespace SSGVoxPuz.Persist {
    public class PreviewLoadMenuOptionHandler : PersistPreviewOptionHandler {
        
        public override void HandleOptionSelected(string voxFileName) {
            PersistanceController.GetSceneLoadInstance().LoadWorld(Path.GetFileNameWithoutExtension(voxFileName));
            PuzMenuControllerComp.GetSceneLoadInstance().NavBack();
            PuzMenuControllerComp.GetSceneLoadInstance().HideMenu();
        }

    }
}
