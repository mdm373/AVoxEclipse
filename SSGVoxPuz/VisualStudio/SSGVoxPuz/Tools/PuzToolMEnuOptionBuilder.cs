using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Utility;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    class PuzToolMenuOptionBuilder : PuzDynaMenuBuilder {

        public GameObject backgroundPrefab;
        public string toolText = "Tools";
        public int labelIndex = 2;

        public override void BuildDynamicItems(GameObject options, PuzScreenLayoutConfig layout) {
            PuzToolController controller = PuzToolController.GetSceneLoadInstance();
            List<PuzTool> activeTools = controller.GetActiveTools();
            for (int i = 0; i < activeTools.Count; i++) {
                PuzToolMenuOption option = options.AddComponent<PuzToolMenuOption>();
                PuzTool tool = activeTools[i];
                option.tool = tool;
                option.HoverText = controller.GetDescription(tool);
                option.IconPrefab = controller.GetToolIconPrefab(tool);
                option.background = Instantiate(backgroundPrefab);
                if (i == labelIndex) {
                    option.DecoText = toolText;
                }
                else {
                    option.DecoText = string.Empty;
                }
            }
        }
    }
}
