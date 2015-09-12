using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using SSGVoxPuz.Tools;

namespace SSGVoxPuz.PuzTutorial.Recording {


    public class PlaybackEvent : TutorialEventComp{
        public bool isLoop;
        public bool isBusyWhilePlaying;
        public PuzRecordingDataComp toPlay;
        public PuzTool resetTool = PuzTool.Pencil;
        
        private PuzRecordingPlayback playback;
        private bool isPlayRequested;

        public override void HandleStarted() {
            playback = PuzRecordingPlayback.GetSceneLoadInstance();
            playback.Play(toPlay);
            PuzController.GetSceneLoadInstance().Faces.Tools.SetTool(resetTool);
            playback.OnFinished += HandleOnFinished;
        }

        private void HandleOnFinished(PuzRecordingPlayback obj) {
            if (isLoop) {
                isPlayRequested = true;
            }
        }

        public override void HandleExited() {
            playback.Stop();
            playback.OnFinished -= HandleOnFinished;
        }

        public override void HandleUpdated() {
            if (isPlayRequested) {
                playback.Play(toPlay);
                PuzController.GetSceneLoadInstance().Faces.Tools.SetTool(resetTool);
                isPlayRequested = false;
            }
        }

        public override void HandleAllFinished() {
            
        }

        public override bool IsBusy() {
            return isBusyWhilePlaying && playback.IsPlaying;
        }
    }
}
