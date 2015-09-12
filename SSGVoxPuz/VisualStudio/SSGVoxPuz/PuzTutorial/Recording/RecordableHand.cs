using System.Collections.Generic;
using SSGCore.Custom;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.Recording {
    public class RecordableHand : CustomBehaviour {
        public List<RecordableFinger> fingers;
        public Transform palm;
        public Transform myTransform;
        
        public HandTransformData BuildHandTransformData() {
            HandTransformData data = new HandTransformData {
                localScale =  myTransform.localScale,
                selfTransform = new TransformData(myTransform),
                palmTransform = new TransformData(palm),
            };
            for (int i = 0; i < fingers.Count; i++) {
                data.fingers.Add(fingers[i].BuildFingerTransformData());
            }
            return data;
        }

        public void PlayBack(HandTransformData handTransformData) {
            handTransformData.selfTransform.AssignTransform(myTransform);
            myTransform.localScale = handTransformData.localScale;
            handTransformData.palmTransform.AssignTransform(palm);
            
            for (int i = 0; i < fingers.Count; i++) {
                fingers[i].PlayBack(handTransformData.fingers[i]);
            }
        }
    }
}
