using System.Collections.Generic;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxPuz.LeapAdapt;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    class PuzHandRecorder {
        
        private readonly PuzHandRecordingConfig config;
        private readonly CustomBehaviour parent;
        private readonly HandControllerWrapper controller;

        private bool isRecording;
        private PuzHandRecordingData activeData;
        private RecordableHand activeHand;
        private Transform handParent;
        private float startTime;
        private float lastRecordingTime;
        private Coroutine activeCoroutine;

        public PuzHandRecorder(PuzHandRecordingConfig aConfig, CustomBehaviour aParent, HandControllerWrapper aController) {
            config = aConfig;
            parent = aParent;
            controller = aController;
        }

        public void StartRecording() {
            if (!isRecording) {
                startTime = Time.time;
                isRecording = true;
                activeData = new PuzHandRecordingData();
                handParent = controller.GetHandParent();
                RecordFrameSample();
                activeCoroutine = parent.StartCoroutine(RunHandRecording());
            }
        }

        public PuzHandRecordingData FinishAndSaveRecording() {
            PuzHandRecordingData data = null;
            if (isRecording) {
                isRecording = false;
                data = activeData;
            }
            return data;
        }

        public void StopRecording() {
            if (isRecording) {
                parent.StopCoroutine(activeCoroutine);
                activeData = null;
                activeHand = null;
                startTime = float.MinValue;
            }
        }

        public void ResetRecording() {
            StopRecording();
            StartRecording();
        }

        private IEnumerator<YieldInstruction> RunHandRecording() {
            while (isRecording) {
                while (Time.time  - lastRecordingTime < config.sampleDuration) {
                    yield return new WaitForEndOfFrame();
                }
                if (isRecording) {
                    RecordFrameSample();
                }
            }
        }

        

        private void RecordFrameSample() {
            lastRecordingTime = Time.time;
            if (CompUtil.IsNull(activeHand)) {
                activeHand = handParent.GetComponentInChildren<RecordableHand>();
            }
            if (!CompUtil.IsNull(activeHand)) {
                HandTransformData transformData = activeHand.BuildHandTransformData();
                PuzHandRecordingDataSample sample = new PuzHandRecordingDataSample {
                    transformData = transformData,
                    deltaTime = Time.time - startTime
                };
                activeData.samples.Add(sample);
            }
        }

        public void Unload() {
            
        }
    }
}
