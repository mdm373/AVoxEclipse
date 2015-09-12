using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    [Serializable]
    public struct PuzScreenLayoutConfig {
        public PuzScreenLayoutType layoutType;
        public List<Vector3> positions;
        public float screenHeight;
        public float screenWidth;
        public float titleHeight;
        public float titlePop;
    }
}
