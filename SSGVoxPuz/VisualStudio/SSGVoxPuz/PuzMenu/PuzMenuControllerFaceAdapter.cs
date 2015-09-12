using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;

namespace SSGVoxPuz.PuzMenu {
    class PuzMenuControllerFaceAdapter : QuickLoadItem, MenuFace {
        private PuzController puzController;
        private PuzMenuControllerComp menuController;

        public override void Load() {
            menuController = PuzMenuControllerComp.GetSceneLoadInstance();
            puzController = PuzController.GetSceneLoadInstance();
            puzController.Faces.Menu = this;
            puzController.OnReset += HanldePuzControllerReset;
        }

        private void HanldePuzControllerReset(PuzController obj) {
            menuController.HideMenu();
        }

        public override void Unload() {
            puzController.OnReset -= HanldePuzControllerReset;
        }

        public MenuFaceMenu Primary { get; set; }
        public MenuFaceMenu Secondary { get; set; }

        public void HideMenus() {
            menuController.HideMenu();
        }
    }
}
