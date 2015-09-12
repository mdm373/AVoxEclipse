using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.TutorialText {
    [Serializable]
    public class GeneralTextBoxConfig {
        public float charsPerSecond = .03f;
        public float newLineWaitTime = 1;
        public PuzButton confirmButton;
        public AudioSource audioSource;
    }
}
