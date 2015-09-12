using SSGCore.Custom;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    public class RecordableFinger : CustomBehaviour {
        public Transform boneOne;
        public Transform boneTwo;
        public Transform boneThree;
        public Transform myTransform;

        public FingerTransformData BuildFingerTransformData() {
            FingerTransformData data = new FingerTransformData {
                boneOneData = new TransformData(boneOne),
                boneTwoData = new TransformData(boneTwo),
                boneThreeData = new TransformData(boneThree),
                selfData = new TransformData(myTransform)
            };
            return data;
        }

        public void PlayBack(FingerTransformData fingerTransformData) {
            fingerTransformData.boneOneData.AssignTransform(boneOne);
            fingerTransformData.boneTwoData.AssignTransform(boneTwo);
            fingerTransformData.boneThreeData.AssignTransform(boneThree);
            fingerTransformData.selfData.AssignTransform(myTransform);
        }
    }
}
