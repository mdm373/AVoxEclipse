using System;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxPuz.LeapAdapt;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {

    
    //Should Load After Puzzle, Requires Puzzle. Requires Puzzle to have loaded hand controller
    public class PuzRecorder : SceneLoadItem, CustomUpdater {
        public KeyCode record = KeyCode.Keypad7;
        public KeyCode finishAndSave = KeyCode.Keypad8;
        public KeyCode resetRecording = KeyCode.Keypad9;
        public PuzInputRecordingConfig inputRecordingConfig;
        public PuzHandRecordingConfig handRecordingConfig;
        public PuzRecordingPlayback linkedPlayback;
        private HandControllerWrapper handController;

        private PuzInputRecorder inputRecorder;
        private PuzHandRecorder handRecorder;
        private bool isRecording;
        private bool isLoaded;

        public void OnDestroy() {
            if (inputRecorder != null) {
                inputRecorder.OnDestroy();
            }
        }

        public void DoUpdate() {
            UpdateForFrame();           
        }

        private void UpdateForFrame() {
            if (Input.GetKeyDown(record)) {
                if (!isRecording) {
                    isRecording = true;
                    Debug.Log("++Recording Started");
                    inputRecorder.StartRecording();
                    handRecorder.StartRecording();
                }
                else {
                    Debug.LogWarning("!!Failed To Start Recording: Recorder is Currently Recording.");
                }
            }
            else if (Input.GetKeyDown(finishAndSave)) {
                FinishAndSave();
            }
            else if (Input.GetKeyDown(resetRecording)) {
                if (isRecording) {
                    Debug.Log("||Recording Reset.");
                    handRecorder.ResetRecording();
                    inputRecorder.ResetRecording();
                }
                else {
                    Debug.LogWarning("!!Failed To Reset Recording: Recorder is Not Currently Recording.");
                }
            }
        }

        private void FinishAndSave() {
            if (isRecording) {
                isRecording = false;
                string recordingName = DateTime.Now.ToLongTimeString();
                Debug.Log("--Recording Stopped And Saved @ " + recordingName);
                PuzInputRecordingData lastRecordedInputs = inputRecorder.FinishAndSaveRecording();
                PuzHandRecordingData lastRecordedMotions = handRecorder.FinishAndSaveRecording();
                PuzRecordingDataComp recording = AddDataToScene(lastRecordedInputs, lastRecordedMotions, recordingName);
                if (linkedPlayback != null) {
                    linkedPlayback.Play(recording);
                }
            }
            else {
                Debug.LogWarning("!!Failed To Stop And Save Recording: Recorder is Not Currently Recording.");
            }
        }

        private PuzRecordingDataComp AddDataToScene(PuzInputRecordingData lastRecordedInputs, PuzHandRecordingData lastRecordedMotions, string recordingName) {
            GameObject obj = new GameObject();
            TransformUtility.ChildAndNormalize(transform, obj.transform);
            PuzRecordingDataComp comp = obj.AddComponent<PuzRecordingDataComp>();
            comp.inputData = lastRecordedInputs;
            comp.motionData = lastRecordedMotions;
            comp.recordingTime = recordingName;
            obj.name = "recorded @ " + recordingName;
            return comp;

        }

        public override void Load() {
            isLoaded = true;
            SceneCustomDelegator.AddUpdater(this);
            handController = PuzController.GetSceneLoadInstance().GetHandController();
            inputRecorder = new PuzInputRecorder(inputRecordingConfig);
            handRecorder = new PuzHandRecorder(handRecordingConfig, this, handController);
        }

        public override void Unload() {
            FinishAndSave();
            SceneCustomDelegator.RemoveUpater(this);
            inputRecorder.Unload();
            handRecorder.Unload();
            isLoaded = false;
        }

        public override bool IsLoaded {
            get { return isLoaded; }
        }

        public override bool IsUnloaded {
            get { return true; }
        }

        public override void OnNextItemLoading() {
            
        }

        public override void OnNexuItemUnloading() {
            
        }
    }
} 
