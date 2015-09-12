using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    class WaitForModelMove : BaseWaitEvent {

        public float requiredDistance = 5;
        public float moveScale = 20;
        public float incrementSize = 1f;

        private float currentDistance;
        private Transform world;
        private int lastIncrement;
        private Vector3 oldPosition;

        protected override float GetCurrentValue() {
            return currentDistance;
        }

        protected override float GetRequiredValue() {
            return requiredDistance;
        }

        protected override void UpdateTrackedValues() {
            float nowDistance = GetNowIncrement();
            if (nowDistance > .001f) {
                currentDistance = currentDistance + (nowDistance * moveScale);
                if (currentDistance > requiredDistance) {
                    currentDistance = requiredDistance;
                }
                oldPosition = world.position;
            }
            lastIncrement = GetCurrentIncrement(currentDistance);
        }

        private float GetNowIncrement() {
            return Mathf.Abs((world.position - oldPosition).magnitude);
        }

        protected override void HandleExtendedStarted() {
            currentDistance = 0;
            world = PuzController.GetSceneLoadInstance().GetPuzzleWorld().transform;
            oldPosition = world.position;
            currentDistance = 0.0f;
        }

        protected override bool ShouldShake() {
            return GetCurrentIncrement(currentDistance + (GetNowIncrement() * moveScale)) > lastIncrement;
        }

        private int GetCurrentIncrement(float aCurrentDistance) {
            return (int)(aCurrentDistance / incrementSize);
        }

        protected override bool IsNowFinished() {
            return currentDistance >= requiredDistance;
        }

        protected override void HandleExtendedExit() {
            
        }
    }
}
