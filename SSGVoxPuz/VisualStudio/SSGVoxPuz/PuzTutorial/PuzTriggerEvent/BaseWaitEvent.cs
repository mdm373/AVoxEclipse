using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using SSGVoxPuz.PuzTutorial.TutorialText;
using SSGVoxPuz.Tools;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    public abstract class BaseWaitEvent : TutorialEventComp {

        public PuzTool activeTool;
        public bool isReset = true;
        public TextBoxType textBoxType = TextBoxType.StaticSmall;
        public Vector2 offset = Vector2.zero;
        public string descriptionText;
        public float attentionDelay = 15;
        
        private TextBox textBox;
        private bool isTransitioning;
        private float transitionStartTime;
        private WaitEventController waitController;
        private bool isExited;
        private bool isNowfinished;
        private bool transitionedOnce;


        public override void HandleStarted() {
            isExited = false;
            waitController = WaitEventController.GetSceneInstance();
            if (isReset) {
                PuzController.GetSceneLoadInstance().Reset();
                waitController.ResetModelState();
                PuzController.GetSceneLoadInstance().Faces.Tools.SetTool(activeTool);
            }
            HandleExtendedStarted();
            textBox = PuzTextController.GetSceneLoadInstance().GetTextBox(textBoxType);
            UpdateTrackedValues();
            UpdateTextBox();
            textBox.AttentionDelay = attentionDelay;
            textBox.Display(offset, false);
            textBox.PopIn();
        }



        private void UpdateTextBox() {
            string text = GetDescriptionText();
            textBox.Text = text;
            textBox.textField.text = GetDescriptionText();
        }

        private string GetDescriptionText() {
            float difference = (GetRequiredValue() - GetCurrentValue());
            return string.Format(descriptionText, difference);
        }

        protected abstract float GetCurrentValue();
        protected abstract float GetRequiredValue();
        protected abstract void UpdateTrackedValues();
        protected abstract void HandleExtendedStarted();
        protected abstract void HandleExtendedExit();
        protected abstract bool ShouldShake();
        protected abstract bool IsNowFinished();

        public override void HandleExited() {
            isExited = true;
            CloseAndReturnTextBox();
        }

        public override void HandleUpdated() {
            isNowfinished = IsNowFinished();
            if (!isTransitioning && ShouldShake() && !isExited && !transitionedOnce) {
                AudioSource.PlayClipAtPoint(waitController.progressSound, transform.position);
                textBox.Shake();
                UpdateTrackedValues();
                UpdateTextBox();
            } else if (isNowfinished && !isTransitioning && !transitionedOnce) {
                transitionedOnce = true;
                AudioSource.PlayClipAtPoint(waitController.finishedSound, transform.position);
                textBox.WinkClosed();
                isTransitioning = true;
                transitionStartTime = Time.time;
            } else if (isTransitioning) {
                if ((Time.time - transitionStartTime) > waitController.transitionTime) {
                    isTransitioning = false;
                }
            }
            else {
                UpdateTrackedValues();
                UpdateTextBox();
            }
        }
    

        public override void HandleAllFinished() {
            CloseAndReturnTextBox();
        }

        public override bool IsBusy() {
            return !isExited && (!isNowfinished || isTransitioning);
        }

        private void CloseAndReturnTextBox() {
            if (textBox != null) {
                textBox.Close();
                PuzTextController.GetSceneLoadInstance().ReturnTextBox(textBox, TextBoxType.StaticSmall);
                textBox = null;
            }
            HandleExtendedExit();
        }
    }
}
