using System;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    [Serializable]
    public class TransformData {
        public Vector3 localPosition;
        public Quaternion localRotation;


        public TransformData() {
            
        }

        public TransformData(Transform model) {
            if(model != null) {
                localPosition = model.localPosition;
                localRotation = model.localRotation;
            }
        }

        public TransformData(TransformData last, TransformData next, float percentage) {
            localPosition = Vector3.Slerp(last.localPosition, next.localPosition, percentage);
            localRotation = Quaternion.Slerp(last.localRotation, next.localRotation, percentage);
        }

        public void AssignTransform(Transform transform) {
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
        }
    }
}
