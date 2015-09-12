using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    [Serializable]
    public class HandTransformData {
        public Vector3 localScale;
        public TransformData selfTransform;
        public TransformData palmTransform;
        public List<FingerTransformData> fingers = new List<FingerTransformData>();

        public HandTransformData() {
            
        }

        public HandTransformData(HandTransformData last, HandTransformData next, float percentage) {
            localScale = Vector3.Slerp(last.localScale, next.localScale, percentage);
            selfTransform = new TransformData(last.selfTransform, next.selfTransform, percentage);
            palmTransform = new TransformData(last.palmTransform, next.palmTransform, percentage);
            int lastFingerCount = last.fingers.Count;
            int nextFingerCount = next.fingers.Count;
            int fingerCount = Math.Min(lastFingerCount, nextFingerCount);
            for (int i = 0; i < fingerCount; i++) {
                fingers.Add(new FingerTransformData(last.fingers[i], next.fingers[i], percentage));
            }
        }
    }
}
