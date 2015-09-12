using System;
using System.Collections.Generic;

namespace SSGVoxPuz.PuzTutorial.TutorialEvent {

    [Serializable]
    public class TutorialEventConfig {
        public string name = "Event";
        public bool isEnabled;
        public List<TutorialEventComp> eventComps;
    }
}
