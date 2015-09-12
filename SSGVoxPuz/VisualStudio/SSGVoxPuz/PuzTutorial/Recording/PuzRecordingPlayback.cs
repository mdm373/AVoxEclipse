using System;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {


    public class PuzRecordingPlayback : SceneSingletonSceneLoadItem<PuzRecordingPlayback> {

        public event Action<PuzRecordingPlayback> OnFinished;
        public PuzHandPlaybackConfig motionPlaybackConfig;
        public TextAsset resetState;
        private PuzInputRecordingPlayback inputPlayBack;
        private PuzHandRecordingPlayback motionPlayBack;
        private bool isPlaying;
        private bool isMotionFinished;
        private bool isInputFinished;
        private bool isLoaded;


        public void Stop() {
            if (isPlaying) {
                Debug.Log("!!Playback Stopped");
                isPlaying = false;
                inputPlayBack.Stop();
                motionPlayBack.Stop();
                isMotionFinished = true;
                isInputFinished = true;
            }
        }

        private void HandleMotionFinish() {
            isMotionFinished = true;
            OnAnyFinish();
        }

        private void HandleInputFinish() {
            isInputFinished = true;
            OnAnyFinish();
        }

        private void OnAnyFinish() {
            if (isMotionFinished && isInputFinished) {
                isPlaying = false;
                if (OnFinished != null) {
                    OnFinished(this);
                }
            }
        }

        public void Play(PuzRecordingDataComp toPlay) {
            if (!isPlaying && toPlay != null) {
                Transform root = PuzController.GetSceneLoadInstance().handControllerParent;
                Debug.Log("++Playback Started");
                isInputFinished = false;
                isMotionFinished = false;
                isPlaying = true;
                if (inputPlayBack.IsPlaying) {
                    inputPlayBack.Stop();
                }
                if (motionPlayBack.IsPlaying) {
                    motionPlayBack.Stop();
                }
                PuzController.GetSceneLoadInstance().Reset(resetState);
                Action onInputFinish = HandleInputFinish;
                Action onMotionFinish = HandleMotionFinish;
                inputPlayBack.Play(toPlay.inputData, onInputFinish);
                motionPlayBack.Play(toPlay.motionData, onMotionFinish, root);
            }
        }

        public override void Load() {
            inputPlayBack = new PuzInputRecordingPlayback(this);
            motionPlayBack = new PuzHandRecordingPlayback(this, motionPlaybackConfig);
            isLoaded = true;
        }

        public override void Unload() {
            Stop();
            isLoaded = false;
        }

        public override bool IsLoaded {
            get { return isLoaded; }
        }

        public override bool IsUnloaded {
            get { return true; }
        }

        public bool IsPlaying { get { return isPlaying; } }

        public override void OnNextItemLoading() {
            
        }

        public override void OnNexuItemUnloading() {
            
        }
    }

    
}
