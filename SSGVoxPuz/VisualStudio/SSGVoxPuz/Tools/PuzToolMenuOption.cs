using SSGCore.Utility;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal.PuzFont;
using SSGVoxPuz.PuzMenu;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.Tools {
    class PuzToolMenuOption : PuzMenuOptionHandlerComp {

        public PuzTool tool = PuzTool.Pencil;
        public GameObject background;
        
        public string DecoText { get; set; }

        public override void HandleOptionSelect() {
            PuzToolController.GetSceneLoadInstance().ActiveTool = tool;
            PuzMenuControllerComp.GetSceneLoadInstance().HideMenu();
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup option) {
            TransformUtility.ChildAndNormalize(icon.transform, background.transform);
            Text textField = background.GetComponentInChildren<Text>();
            textField.text = DecoText;
            textField.font = PuzFontController.GetSceneLoadInstance().GetFont(PuzFontType.Standard);
        }
    }
}
