using System;
using System.Collections.Generic;
using SSGCore.Custom;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    class PuzInputRecordingPlayback {
        
        private readonly CustomBehaviour parent;
        private bool isPlaying;
        private Action onFinishedCallback;
        private Coroutine activeCoroutine;

        public PuzInputRecordingPlayback(CustomBehaviour aParent) {
            parent = aParent;
        }

        public bool IsPlaying { get { return isPlaying;} }

        public void Play(PuzInputRecordingData recordingData, Action onFinished) {
            if (!isPlaying) {
                onFinishedCallback = onFinished;
                isPlaying = true;
                activeCoroutine = parent.StartCoroutine(PlayBackRecordedData(recordingData));
            }
        }


        public IEnumerator<YieldInstruction> PlayBackRecordedData(PuzInputRecordingData data) {
            
            PuzButtonController.IsListiningToInput = false;
            float startTime = Time.time;
            List<PuzInputRecordingDataSample> samples = data.samples;
            for(int i = 0; i < samples.Count; i++){
                PuzInputRecordingDataSample currentSample = samples[i];
                float deltaTime = Time.time - startTime;
                while (currentSample.deltaTime > deltaTime) {
                    yield return new WaitForEndOfFrame();
                    deltaTime = Time.time - startTime;
                }

                
                ProcessCurrentSampleNow(currentSample);
            }
            PuzButtonController.IsListiningToInput = true;
            isPlaying = false;
            onFinishedCallback();
            onFinishedCallback = null;
        }


        private void ProcessCurrentSampleNow(PuzInputRecordingDataSample currentSample) {
            List<PuzButtonEventData> events = currentSample.buttonEvents;
            for (int i = 0; i < events.Count; i++) {
                PuzButtonEventData anEvent = events[i];
                PuzButtonController.FireEvent(anEvent);
            }
        }

        public void Stop() {
            if (isPlaying) {
                PuzButtonController.IsListiningToInput = true;
                isPlaying = false;
                parent.StopCoroutine(activeCoroutine);
                onFinishedCallback = null;
            }
        }
    }
}
