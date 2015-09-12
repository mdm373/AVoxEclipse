

namespace SSGVoxPuz.PuzTutorial.TutorialEvent {
    
    public class TutorialEventController {
        private readonly TutorialEventConfig eventConfig;
        private TutorialEventComp eventComp;
        private int eventIndex;
        private bool isBusy;

        public TutorialEventController(TutorialEventConfig aEventConfig) {
            eventConfig = aEventConfig;
        }

        public void DoUpdate() {
            if (eventComp != null) {
                if (eventComp.IsBusy()) {
                    eventComp.HandleUpdated();
                }
                if (eventComp != null && !eventComp.IsBusy()) {
                    AdvanceEventComp();
                    DoUpdate();
                }
            }
            else {
                TellAllEventCompsFinished();
                isBusy = false;
            }
        }

        public void HandleStarted() {
            isBusy = true;
            AdvanceEventComp();
        }

        private void AdvanceEventComp() {
            if (eventComp != null) {
                eventComp.HandleExited();
            }
            if (eventIndex < eventConfig.eventComps.Count) {
                eventComp = eventConfig.eventComps[eventIndex];
                eventIndex++;
                eventComp.HandleStarted();
            }
            else {
                eventComp = null;
            }
        }

        public void HandleEnded() {
            if (eventComp != null) {
                eventComp.HandleExited();
            }
            TellAllEventCompsFinished();
        }

        public void HandleSkipPressed() {
            eventComp.HandleExited();
            isBusy = false;
            TellAllEventCompsFinished();

        }

        private void TellAllEventCompsFinished() {
            for (int i = 0; i < eventConfig.eventComps.Count; i++) {
                eventConfig.eventComps[i].HandleAllFinished();
            }
        }

        public bool IsBusy() {
            return isBusy;
        }
    }
}
