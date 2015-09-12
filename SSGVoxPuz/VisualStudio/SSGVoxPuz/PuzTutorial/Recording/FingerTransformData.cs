using System;

namespace SSGVoxPuz.PuzTutorial.Recording {
    [Serializable]
    public class FingerTransformData {
        public TransformData boneOneData;
        public TransformData boneTwoData;
        public TransformData boneThreeData;
        public TransformData selfData;

        public FingerTransformData() {
            
        }

        public FingerTransformData(FingerTransformData last, FingerTransformData next, float percentage) {
            boneOneData = new TransformData(last.boneOneData, next.boneOneData, percentage);
            boneTwoData = new TransformData(last.boneTwoData, next.boneTwoData, percentage);
            boneThreeData = new TransformData(last.boneThreeData, next.boneThreeData, percentage);
            selfData = new TransformData(last.selfData, next.selfData, percentage);
        }
    }
}

