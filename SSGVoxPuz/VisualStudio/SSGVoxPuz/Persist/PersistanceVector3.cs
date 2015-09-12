using System;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    [Serializable]
    public struct PersistanceVector3 {
        public float x;
        public float y;
        public float z;

        public static PersistanceVector3 FromVector3(Vector3 value) {
            PersistanceVector3 dataPoint = new PersistanceVector3 {
                x = value.x,
                y = value.y,
                z = value.z
            };
            return dataPoint;
        }

        public Vector3 ToVector3() {
            Vector3 vector = new Vector3(x,y,z);
            return vector;
        }

        public object PrintInfo() {
            return "["+ x + "-" + y + "-" + z+"]";
        }
    }
}
