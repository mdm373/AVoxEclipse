using System;
using SSGCore.Custom;
using UnityEngine;

namespace SSGVoxPuz.LeapAdapt {
    public class HandControllerWrapper : CustomBehaviour {
        private string savedRecording;
        
        private Transform handParent;
        private const string RECORD = "AdaptedRecord";
        private const string PLAY = "AdaptedPlayRecording";
        private const string FINISH_AND_SAVE = "AdaptedFinishAndSaveRecording";
        private const string RESET = "AdaptedResetRecording";
        private const string GET_HAND_PARENT = "AdaptedGetHandParent";
        private const string SET_HAND_PARENT = "AdaptedSetHandParent";
        private const string TRACKING_ENABLED = "AdaptedTrackingEnabled";

        public void Record() {
            SendMessage(RECORD, SendMessageOptions.RequireReceiver);
        }

        public string FinishAndSaveRecording() {
            Action<string> saveAction = SaveRecordingCallBack;
            SendMessage(FINISH_AND_SAVE, saveAction, SendMessageOptions.RequireReceiver);
            return savedRecording;
        }

        public void ResetRecording() {
            SendMessage(RESET, SendMessageOptions.RequireReceiver);
        }

        public void PlayRecording(TextAsset recording) {
            SendMessage(PLAY, recording, SendMessageOptions.RequireReceiver);
        }

        public Transform GetHandParent() {
            Action<Transform> handParentCallBack = HandParentCallBack;
            SendMessage(GET_HAND_PARENT, handParentCallBack, SendMessageOptions.RequireReceiver);
            return handParent;
        }

        private void SaveRecordingCallBack(string saveFile) {
            savedRecording = saveFile;
        }

        private void HandParentCallBack(Transform aHandParent) {
            handParent = aHandParent;
        }

        public void SetHandParent(Transform handControllerParent) {
            SendMessage(SET_HAND_PARENT, handControllerParent, SendMessageOptions.RequireReceiver);
        }

        public void SetTrackingEnabled(bool isEnabled) {
            SendMessage(TRACKING_ENABLED, isEnabled, SendMessageOptions.RequireReceiver);
        }
    }
}
