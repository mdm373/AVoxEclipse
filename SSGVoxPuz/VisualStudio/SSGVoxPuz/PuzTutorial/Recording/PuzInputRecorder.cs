using System;
using System.Collections.Generic;
using System.Linq;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    class PuzInputRecorder {
        private readonly PuzInputRecordingConfig config;
        
        private bool isRecording;
        private PuzInputRecordingData activeRecordingData;
        private float recordingStartTime;
        private Stack<PuzInputRecordingDataSample> recordingStack;
        
        public PuzInputRecorder(PuzInputRecordingConfig aConfig) {
            config = aConfig;
            
            PuzButton[] buttons = (PuzButton[])Enum.GetValues(typeof(PuzButton));
            for (int buttonIndex = 0; buttonIndex < buttons.Length; buttonIndex++) {
                PuzButtonDriverType[] driverTypes = (PuzButtonDriverType[]) Enum.GetValues(typeof (PuzButtonDriverType));
                for (int driverIndex = 0; driverIndex < driverTypes.Length; driverIndex++) {
                    PuzButtonController.AddListener(buttons[buttonIndex], driverTypes[driverIndex], OnInputEvent);
                }
            }
        }

        public void OnDestroy() {
            PuzButton[] buttons = (PuzButton[])Enum.GetValues(typeof(PuzButton));
            for (int buttonIndex = 0; buttonIndex < buttons.Length; buttonIndex++) {
                PuzButtonDriverType[] driverTypes = (PuzButtonDriverType[])Enum.GetValues(typeof(PuzButtonDriverType));
                for (int driverIndex = 0; driverIndex < driverTypes.Length; driverIndex++) {
                    PuzButtonController.RemoveListener(buttons[buttonIndex], driverTypes[driverIndex], OnInputEvent);
                }
            }
        }

        private void OnInputEvent(PuzButtonEventData eventdata) {
            PushRecordingItem(eventdata);
        }

        public void StartRecording() {
            if (!isRecording) {
                isRecording = true;
                activeRecordingData = new PuzInputRecordingData();
                recordingStack = new Stack<PuzInputRecordingDataSample>();
                recordingStartTime = Time.time;
            }
            
        }

        public PuzInputRecordingData FinishAndSaveRecording() {
            PuzInputRecordingData recordedData = null;
            if (isRecording) {
                isRecording = false;
                int stackSize = recordingStack.Count;
                PuzInputRecordingDataSample[] recordedSamples = new PuzInputRecordingDataSample[stackSize];
                for (int i = stackSize - 1; i >= 0; i--) {
                    recordedSamples[i] = recordingStack.Pop();
                }
                activeRecordingData.samples = recordedSamples.ToList();
                recordedData = activeRecordingData;
            }
            return recordedData;
        }

        public void ResetRecording() {
            StopRecording();
            StartRecording();
        }

        public void StopRecording() {
            isRecording = false;
            recordingStack = null;
            activeRecordingData = null;
        }

        private void PushRecordingItem(PuzButtonEventData eventData) {
            if (isRecording) {
                float now = Time.time;
                float deltaTime = now - recordingStartTime;
                bool wasQueueHeadUsed = false;
                if (recordingStack.Any()) {
                    PuzInputRecordingDataSample sample = recordingStack.Peek();
                    if ( (now - sample.deltaTime) <= config.sampleDuration) {
                        sample.buttonEvents.Add(eventData);
                        wasQueueHeadUsed = true;
                    }
                }
                if (!wasQueueHeadUsed) {
                    PuzInputRecordingDataSample sample = new PuzInputRecordingDataSample { deltaTime = deltaTime };
                    sample.buttonEvents.Add(eventData);
                    recordingStack.Push(sample);
                }
            }
        }

        public void Unload() {
            
        }
    }
}
