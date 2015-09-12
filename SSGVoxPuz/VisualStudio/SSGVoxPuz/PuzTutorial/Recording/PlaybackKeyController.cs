using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    class PlaybackKeyController : CustomBehaviour, CustomUpdater {

        public PuzRecordingPlayback playback;
        public PuzRecordingDataComp toPlay;
        public KeyCode playKey = KeyCode.Keypad4;
        public KeyCode stopKey = KeyCode.Keypad5;

        public void OnEnable() {
            SceneCustomDelegator.AddUpdater(this);
        }

        public void DoUpdate() {

            if (Input.GetKeyDown(playKey)) {
                playback.Play(toPlay);
            }
            else if (Input.GetKeyDown(stopKey)) {
                playback.Stop();
            }
        }
    }
}
