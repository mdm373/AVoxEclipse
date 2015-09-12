using System;
using System.Collections.Generic;
using SSGCore.Custom;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.Environment {
    public class CameraVeilFade : CustomBehaviour {

        public Image veil;
        public TextTrailComp trailingText;
        private Coroutine activeCoroutine;
        public float duration = 1;
        public bool IsFading { get; private set; }
        public event Action<CameraVeilFade> OnFadeFinish;

        public void FadeIn() {
            if (trailingText != null) {
                trailingText.StartTrailing();
            }
            IsFading = true;
            StopActiveCoroutine();
            activeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f));
        }

        public void FadeToBlack() {
            IsFading = true;
            StopActiveCoroutine();
            activeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f));
        }
        
        private void StopActiveCoroutine() {
            if (activeCoroutine != null) {
                StopCoroutine(activeCoroutine);
                IsFading = false;
                activeCoroutine = null;
            }
        }

        private void FireFadeFinish() {
            if (trailingText != null) {
                trailingText.StartTrailing();
            }
            if (OnFadeFinish != null) {
                OnFadeFinish(this);
            }
        }

        private IEnumerator<YieldInstruction> FadeCoroutine(float from, float to) {
            Color color = veil.color;
            color.a = from;
            veil.color = color;
            float startTime = Time.time;
            while ( (Time.time - startTime) <= duration) {
                color.a = Mathf.Lerp(from, to, (Time.time - startTime)  / duration);
                yield return new WaitForEndOfFrame();
                veil.color = color;
            }
            IsFading = false;
            color.a = to;
            veil.color = color;
            activeCoroutine = null;
            FireFadeFinish();
        }

    }
}
