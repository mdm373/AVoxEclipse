using System.Collections.Generic;

namespace SSGVoxPuz.PuzTutorial.TutorialEvent {
    class CompoundEvent : TutorialEventComp {

        public List<TutorialEventComp> events;
        
        public override void HandleStarted() {
            for (int i = 0; i < events.Count; i++) {
                events[i].HandleStarted();
            }
        }

        public override void HandleExited() {
            for (int i = 0; i < events.Count; i++) {
                events[i].HandleExited();
            }
        }

        public override void HandleUpdated() {
            for (int i = 0; i < events.Count; i++) {
                events[i].HandleUpdated();
            }
        }

        public override void HandleAllFinished() {
            for (int i = 0; i < events.Count; i++) {
                events[i].HandleAllFinished();
            }
        }

        public override bool IsBusy() {
            bool isBusy = false;
            for (int i = 0; i < events.Count; i++) {
                isBusy = events[i].IsBusy();
                if (isBusy) {
                    break;
                }
            }
            return isBusy;
        }
    }
}
