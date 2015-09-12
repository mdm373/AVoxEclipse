using System;
using System.Collections.Generic;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SSGVoxPuz.PuzTutorial.Recording {
    class PuzHandRecordingPlayback {
        private readonly CustomBehaviour parent;
        private readonly PuzHandPlaybackConfig config;
        private GameObject activeHandObject;
        private RecordableHand activeRecordableHand;
        private Action onFinishedCallBack;
        private Coroutine activeCoroutine;

        public PuzHandRecordingPlayback(CustomBehaviour aParent, PuzHandPlaybackConfig aConfig) {
            parent = aParent;
            config = aConfig;
        }

        public bool IsPlaying { get; private set; }

        public void Play(PuzHandRecordingData motionData, Action onFinished, Transform root) {
            if (!IsPlaying) {
                PuzController.GetSceneLoadInstance().GetHandController().SetTrackingEnabled(false);
                onFinishedCallBack = onFinished;
                IsPlaying = true;
                activeHandObject = Object.Instantiate(config.handPrefab);
                activeRecordableHand = activeHandObject.GetComponentInChildren<RecordableHand>();
                TransformUtility.ChildAndNormalize(root, activeHandObject.transform);
                activeCoroutine = parent.StartCoroutine(PlayBack(motionData));
            }
        }

        public void Stop() {
            if (IsPlaying) {
                CommonStop();
                parent.StopCoroutine(activeCoroutine);
                onFinishedCallBack = null;
            }
        }

        private void CommonStop() {
            PuzController.GetSceneLoadInstance().GetHandController().SetTrackingEnabled(true);
            DestroyUtility.DestroyAsNeeded(activeHandObject);
            IsPlaying = false;
            activeHandObject = null;
            activeRecordableHand = null;
        }

        private IEnumerator<YieldInstruction> PlayBack(PuzHandRecordingData motionData) {
            float startTime = Time.time;
            int sampleCount = motionData.samples.Count;
            if (sampleCount > 2) {
                for (int i = 0; i < sampleCount - 2; i++) {
                    PuzHandRecordingDataSample lastSample = motionData.samples[i];
                    PuzHandRecordingDataSample nextSample = motionData.samples[i + 1];
                    float currentDelta = Time.time - startTime;
                    while (nextSample.deltaTime >= currentDelta) {
                        currentDelta = Time.time - startTime;
                        PlayBackSample(currentDelta, lastSample, nextSample);
                        yield return new WaitForEndOfFrame();
                    }
            }
            }
            CommonStop();
            onFinishedCallBack();
            onFinishedCallBack = null;
        }

        private void PlayBackSample(float currentDelta, PuzHandRecordingDataSample lastSample, PuzHandRecordingDataSample nextSample) {
            float sampleDelta = nextSample.deltaTime  - lastSample.deltaTime;
            float deltaAfterLast = currentDelta - lastSample.deltaTime;
            float percentage = 0;
            if (sampleDelta > 0) {
                percentage = (deltaAfterLast > sampleDelta) ? 1 : deltaAfterLast / sampleDelta;
            }
            HandTransformData scaledHandTransformData = new HandTransformData(
                lastSample.transformData, 
                nextSample.transformData, 
                percentage);
            activeRecordableHand.PlayBack(scaledHandTransformData);


        }
    }
}
