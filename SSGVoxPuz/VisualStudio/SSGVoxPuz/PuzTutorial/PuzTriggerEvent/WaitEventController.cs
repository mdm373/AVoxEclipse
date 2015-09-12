using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    class WaitEventController : SceneSingletonBehaviour<WaitEventController> {

        public TextAsset resetState;
        public AudioClip finishedSound;
        public double transitionTime;
        public AudioClip progressSound;

        public void ResetModelState() {
            if (resetState != null) {
                PuzController.GetSceneLoadInstance().Faces.Persist.LoadWorld(resetState);
            }
        }
    }
}
