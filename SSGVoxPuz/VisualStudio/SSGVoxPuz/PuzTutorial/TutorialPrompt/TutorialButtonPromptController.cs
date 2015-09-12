using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialPrompt {
    class TutorialButtonPromptController : SceneSingletonQuickLoadItem<TutorialButtonPromptController> {

        public TutorialButtonPrompt promptPrefab;

        public override void Load() {
            
        }

        public override void Unload() {
            
        }

        public TutorialButtonPrompt GetPrompt() {
            GameObject promptObj = Instantiate(promptPrefab.gameObject);
            TutorialButtonPrompt prompt = promptObj.GetComponent<TutorialButtonPrompt>();
            return prompt;
        }

        public void ReturnPrompt(TutorialButtonPrompt prompt) {
            DestroyUtility.DestroyAsNeeded(prompt.gameObject);
        }
    }
}
