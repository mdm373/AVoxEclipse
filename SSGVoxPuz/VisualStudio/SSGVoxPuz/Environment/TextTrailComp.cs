using System.Collections.Generic;
using SSGCore.Custom;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.Environment {
    public class TextTrailComp : CustomBehaviour{
        
        public Text text;
        public string baseValue;
        public float delay = 1;
        public float maxTrailing = 4;

        private Coroutine activeCoroutine;
        private bool isRunning;
        private bool wasRendered;

        public void StartTrailing() {
            isRunning = true;
            text.text = baseValue;
            activeCoroutine = StartCoroutine(RunTrail());
        }

        public void StopTrailing() {
            isRunning = false;
            if (activeCoroutine != null) {
                StopCoroutine(activeCoroutine);
                text.text = baseValue;
            }
        }

        public void OnRenderObject() {
            wasRendered = true;
        }

        private IEnumerator<YieldInstruction> RunTrail() {
            int currentCount = 0;
            while (isRunning) {
                wasRendered = false;
                string suffix = string.Empty;
                for (int i = 0; i < currentCount; i++) {
                    suffix = suffix + '.';
                }
                text.text = baseValue + suffix;
                while (!wasRendered) {
                    yield return new WaitForEndOfFrame();
                }
                float elapsedTime = 0.0f;
                while (elapsedTime < delay) {
                    yield return new WaitForEndOfFrame();
                    elapsedTime += Time.deltaTime;
                }
                currentCount++;
                if (currentCount > maxTrailing) {
                    currentCount = 0;
                }
            }
        }
    }
}
