using System;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {

    [Serializable]
    public class PuzMenuSelectionConfig {

        public LayerMask selectionLayerMask;
        public float maxDistance;
        public PuzButton selectionButton;
        public PuzButton alternateSelectionButton = PuzButton.Secondary;
    }
}
