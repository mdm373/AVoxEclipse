using System;
using UnityEngine;

namespace SSGVoxPuz.Interaction {
    
    [Serializable]
    public class DragIndicationConfig {
        public float minUiPercent;
        public float maxUiDistance;
        public float scaledValue;
        public float responseFactor;
        public float relativeUiScale;

        public GameObject rootIndicatorPrefab;
        public GameObject scaledIndicatorPrefab;
        public GameObject tipIndicatorPrefab;
    }
}
