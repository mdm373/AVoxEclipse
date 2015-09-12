using SSGCore.Custom;

namespace SSGVoxPuz.PuzTutorial.TutorialEvent {
    public abstract class TutorialEventComp : CustomBehaviour {

        public abstract void HandleStarted();
        public abstract void HandleExited();
        public abstract void HandleUpdated();
        public abstract void HandleAllFinished();
        public abstract bool IsBusy();

    }
}
