using System.Collections.Generic;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialText {
    public class ScrollingTextBox : TextBox{
        
        
        private Coroutine activeCoroutine;
        
        private void ResetTextScroll() {
            Application.targetFrameRate = 60;
            if (activeCoroutine != null) {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(ScrollText(GetFormattedText()));
        }

        private IEnumerator<YieldInstruction> ScrollText(string toDisplay) {
            textField.text = string.Empty;
            int textIndex = 0;
            float lastUpdateTime = Time.time;
            while (textIndex < toDisplay.Length) {
                while (Time.time - lastUpdateTime < GeneralConfig.charsPerSecond) {
                    yield return  new WaitForEndOfFrame();
                }
                textIndex = UpdateTextIndex(toDisplay, lastUpdateTime, textIndex);
                lastUpdateTime = Time.time;
                
                while (textIndex <= toDisplay.Length && textIndex >= 0 && char.IsWhiteSpace(toDisplay, textIndex-1)) {
                    if ('\n'.Equals(toDisplay[textIndex-1])) {
                        float newLineWaitStart = Time.time;
                        while (Time.time - newLineWaitStart < GeneralConfig.newLineWaitTime) {
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    lastUpdateTime = Time.time;
                    textIndex++;
                    
                }
                int length = toDisplay.Length - textIndex;
                if (length + textIndex <= toDisplay.Length) {
                    string trailing = (length > 0) ? toDisplay.Substring(textIndex, length) : string.Empty;
                    if (textIndex < toDisplay.Length) {
                        textField.text = toDisplay.Substring(0, textIndex) + "<color=#00000000>" + trailing + "</color>";
                    }
                    GeneralConfig.audioSource.Play();
                }
                if (textIndex == toDisplay.Length - 1) {
                    break;
                }
            }
            FireFinished();
        }

        private int UpdateTextIndex(string toDisplay, float lastUpdateTime, int textIndex) {
            float elapsed = Time.time - lastUpdateTime;
            elapsed = elapsed/GeneralConfig.charsPerSecond;
            int textIncrement = (int) elapsed;
            textIndex += textIncrement;
            if (textIndex >= toDisplay.Length - 2) {
                textIndex = toDisplay.Length - 1;
            }
            return textIndex;
        }

        protected override void DisplayExtended() {
            ResetTextScroll();
            PuzButtonController.AddListener(GeneralConfig.confirmButton, PuzButtonDriverType.ShortPress, HandleSkipPressed);
        }

        private void HandleSkipPressed(PuzButtonEventData eventdata) {
            if (eventdata.eventType == PuzButtonEventType.PressConfirmed) {
                StopScrolling();
                textField.text = GetFormattedText();
            } 
        }

        protected override void CloseExtended() {
            StopScrolling();
            PuzButtonController.RemoveListener(GeneralConfig.confirmButton, PuzButtonDriverType.ShortPress, HandleSkipPressed);

        }

        private void StopScrolling() {
            if (activeCoroutine != null) {
                StopCoroutine(activeCoroutine);
                FireFinished();
            }
        }

    }
}
