using System;
using UnityEngine;

namespace SSGVoxPuz.PuzMod {
    [Serializable]
    public class ModHandlerConfig {
        public float speed;
        public float arrivalTolerance;
        public float variance;
        public float airVariance;
        public float airSpeed;
        public float airSpin;
        public Material modelMaterial;
    }
}
