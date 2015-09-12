using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using UnityEngine;
using UnityEngine.Audio;

namespace SSGVoxPuz.PuzAudio {
    public class AudioController : SceneSingletonBehaviour<AudioController>, CustomUpdater {

        public bool isBgmPlaying;
        public AudioSource bgmAudioSource;
        public AnimationCurve fade;
        public float fadeOutTime = 2.5f;

        private bool isFading;
        private bool wasBgmPlaying;
        private bool wasFading;
        private float fadeStartTime;
        private int fadeStartValue;
        private int fadeEndValue;

        public void OnEnable() {
            SceneCustomDelegator.AddUpdater(this);
        }

        public void SetBgm(AudioClip clip) {
            bgmAudioSource.clip = clip;
        }


        public void DoUpdate() {
            if (isBgmPlaying && !wasBgmPlaying) {
                isFading = true;
                fadeStartTime = Time.time;
                fadeStartValue = 0;
                fadeEndValue = 1;

            }
            if (!isBgmPlaying && wasBgmPlaying) {
                isFading = true;
                fadeStartTime = Time.time;
                fadeStartValue = 1;
                fadeEndValue = 0;
            }
            if (isFading) {
                float fadePercent = (Time.time- fadeStartTime) / fadeOutTime;
                if(fadePercent >= 1) {
                    isFading = false;
                }
                fadePercent = fade.Evaluate(fadePercent);
                float currentFadeValue = Mathf.SmoothStep(fadeStartValue, fadeEndValue, fadePercent);
                bgmAudioSource.volume = currentFadeValue;
            }
            if (!isFading && !isBgmPlaying && wasFading) {
                bgmAudioSource.Pause();
            }
            if (isFading && !wasFading && isBgmPlaying) {
                bgmAudioSource.Play();
            }
            wasFading = isFading;
            wasBgmPlaying = isBgmPlaying;
        }
    }
}
