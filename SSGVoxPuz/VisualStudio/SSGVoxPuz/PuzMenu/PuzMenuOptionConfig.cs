using System;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {

    [Serializable]
    public class PuzMenuOptionConfig {
        public bool isHoverable = true;
        public bool isSelectable = true;
        public Color backgroundNormal;
        public Color backgroundHover;
        public string screenTitle = "Title";
        public AudioClip selectSound;
        public AudioClip hoverSound;
    }
}
