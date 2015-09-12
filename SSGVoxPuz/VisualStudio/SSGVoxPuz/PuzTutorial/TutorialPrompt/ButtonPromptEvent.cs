using SSGVoxPuz.PuzInput;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialPrompt {
    class ButtonPromptEvent : TutorialEventComp {

        public PuzButton button = PuzButton.Primary;
        public string text = "Confirm";
        public Vector2 offset = Vector2.zero;
        public Vector3 viewPosition = Vector3.zero;
        public bool isBusyTillPressed = true;
        public bool shouldListen = true;
        private bool isPressed;
        private TutorialButtonPrompt prompt;

        public override void HandleStarted() {
            isPressed = false;
            prompt = TutorialButtonPromptController.GetSceneLoadInstance().GetPrompt();
            if (shouldListen) {
                prompt.OnPromptConfirmed += HandlePromptConfirmed;
            }
            prompt.Display(offset, viewPosition, text, button, shouldListen);

        }

        private void HandlePromptConfirmed(TutorialButtonPrompt obj) {
            isPressed = true;
        }

        public override void HandleExited() {
            CleanUp();
        }

        private void CleanUp() {
            isPressed = true;
            if (prompt != null) {
                prompt.OnPromptConfirmed -= HandlePromptConfirmed;
                prompt.Close();
                TutorialButtonPromptController.GetSceneLoadInstance().ReturnPrompt(prompt);
            }
        }

        public override void HandleUpdated() {
                    
        }

        public override void HandleAllFinished() {
            CleanUp();
        }

        public override bool IsBusy() {
            return !isPressed && isBusyTillPressed;
        }
    }
}
