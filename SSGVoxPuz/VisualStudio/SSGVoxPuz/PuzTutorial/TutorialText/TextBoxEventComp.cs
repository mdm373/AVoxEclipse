using SSGHud;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialText {
    public class TextBoxEventComp : TutorialEventComp {

        [HideInInspector] public string textToDisplay = "hello world";
        public TextBoxType textBoxType = TextBoxType.Scrolling;
        public Vector2 offset = Vector2.zero;
        public bool isBusyForTextWork = true;
        public bool isBreathing;
        public bool isCloseOnEventExit;
        public bool isCentered;
        public bool isPopIn;
        public int fontSize = 40;
        public float attentionDelay = 15;

        private TextBox textBox;
        private bool isBusy;
        public override void HandleStarted() {
            isBusy = true;
            textBox = PuzTextController.GetSceneLoadInstance().GetTextBox(textBoxType);
            textBox.AttentionDelay = attentionDelay;
            textBox.Text = textToDisplay;
            textBox.OnFinished += HandleTextBoxFinished;
            textBox.FontSize = fontSize;
            textBox.Display(offset, isCentered);
            textBox.SetBreathing(isBreathing);
            if (isPopIn) {
                textBox.PopIn();
            }
        }

        private void HandleTextBoxFinished(TextBox obj) {
            obj.OnFinished -= HandleTextBoxFinished;
            isBusy = false;
        }

        private void ReturnTextBox() {
            PuzTextController.GetSceneLoadInstance().ReturnTextBox(textBox, textBoxType);
        }

        public override void HandleExited() {
            if (textBox.IsDisplayed && isCloseOnEventExit) {
                CleanUp();
            }
        }

        private void CleanUp() {
            textBox.OnFinished -= HandleTextBoxFinished;
            textBox.Close();
            ReturnTextBox();
            isBusy = false;
        }

        public override void HandleUpdated() {
            
        }

        public override void HandleAllFinished() {
            if (textBox.IsDisplayed) {
                CleanUp();
            }
        }

        public override bool IsBusy() {
            bool isBusyNow = false;
            if (isBusyForTextWork) {
                isBusyNow = isBusy;
            }
            return isBusyNow;
        }
    }
}
