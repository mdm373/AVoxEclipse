using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;

namespace SSGVoxPuz.PuzTutorial.TutorialPrompt {
    class PromptQuickLoad : QuickLoadItem {

        public ButtonPromptEvent prompt;

        public override void Load() {
            prompt.HandleStarted();
        }

        public override void Unload() {
            prompt.HandleExited();
        }
    }
}
