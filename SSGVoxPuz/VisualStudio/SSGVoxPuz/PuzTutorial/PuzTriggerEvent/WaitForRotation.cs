using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    public class WaitForRotation : BaseWaitEvent {

        public float requiredDegrees;
        public float incrementSize = 1;
        
        private Quaternion oldRotation;
        private float currentRotationChange;
        private Transform world;
        

        protected override float GetCurrentValue() {
            return currentRotationChange;
        }

        protected override float GetRequiredValue() {
            return requiredDegrees;
        }

        protected override void UpdateTrackedValues() {
            float nowChange = GetNowChange();
            if (nowChange > .001f) {
                currentRotationChange = nowChange + currentRotationChange;
                if (currentRotationChange > requiredDegrees) {
                    currentRotationChange = requiredDegrees;
                }
            }
            oldRotation = world.rotation;
        }

        private float GetNowChange() {
            return Mathf.Abs(Quaternion.Angle(world.rotation, oldRotation));
        }

        protected override void HandleExtendedStarted() {
            world = PuzController.GetSceneLoadInstance().GetPuzzleWorld().transform;
            currentRotationChange = 0.0f;
            oldRotation = world.rotation;
        }

        protected override bool ShouldShake() {
            return GetCurrentIncrement(GetNowChange() + currentRotationChange) >
                   GetCurrentIncrement(currentRotationChange);
        }

        protected override bool IsNowFinished() {
            return currentRotationChange >= requiredDegrees;
        }

        protected override void HandleExtendedExit() {
            
        }

        private int GetCurrentIncrement(float value) {
            return (int)(value / incrementSize);
        }
        
    }
}
