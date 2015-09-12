using SSGCore.Custom;
using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.Interaction {
    class DragIndicationController {
        
        private DragIndicationConfig config;
        private Transform activeGesture;
        private Vector3 startPosition;
        private GameObject rootIndicator;
        private GameObject tipIndicator;
        private GameObject scaledIndicator;
        private GameObject rootObject;

        public DragIndicationConfig Config { set { config = value; } }
        public bool IsDragging { get; private set; }
        public float DragScale { get; private set; }
        public Vector3 DragDirection { get; private set; }
        public float CurrentValue { get; private set; }
        public Vector3 CurrentValueVector { get; private set; }

        public void OnLoad(CustomBehaviour aParent) {
            if (CompUtil.IsNull(rootObject)) {
                rootObject = new GameObject("drag-indicator");
            }
            TransformUtility.ChildAndNormalize(aParent.transform, rootObject.transform);
            InstanceIfNull(ref rootIndicator, config.rootIndicatorPrefab);
            InstanceIfNull(ref scaledIndicator, config.scaledIndicatorPrefab);
            InstanceIfNull(ref tipIndicator, config.tipIndicatorPrefab);
            rootObject.SetActive(false);
        }

        private void InstanceIfNull(ref GameObject instance, GameObject prefab) {
            if (CompUtil.IsNull(instance)) {
                instance = Object.Instantiate(prefab);
            }
            TransformUtility.ChildAndNormalize(rootObject.transform, instance.transform);
        }

        public void StartDragging(Transform gesture, Vector3 start) {
            rootObject.SetActive(true);
            rootObject.transform.position = start;
            activeGesture = gesture;
            startPosition = start;
            IsDragging = true;
            CurrentValue = 0.0f;
            CurrentValueVector = Vector3.zero;
            tipIndicator.transform.position = startPosition;
            Vector3 localScale = scaledIndicator.transform.localScale;
            localScale.z = 0;
            scaledIndicator.transform.localScale = localScale;
        }

        public void StopDragging() {
            rootObject.SetActive(false);
            IsDragging = false;
            DragDirection = Vector3.zero;
            activeGesture = null;
            DragScale = 0.0f;
        }


        public void DoUpdate() {
            float calcValue = 0.0f;
            Vector3 calcValueVector = Vector3.zero;
            if (IsDragging) {
                Debug.DrawLine(activeGesture.position, startPosition, Color.red);
                DragDirection = activeGesture.position - startPosition;
                float directionMag = DragDirection.magnitude;
                DragDirection = DragDirection.normalized;

                DragScale = directionMag/config.maxUiDistance;
                DragScale = (DragScale > 1) ? 1 : DragScale;

                if (DragScale > config.minUiPercent) {
                    rootObject.transform.rotation = Quaternion.LookRotation(DragDirection, rootObject.transform.up);
                    rootObject.transform.forward = DragDirection;
                    tipIndicator.transform.position = startPosition + (DragScale*config.maxUiDistance)* DragDirection;
                    Vector3 localScale = scaledIndicator.transform.localScale;
                    localScale.z = config.relativeUiScale * DragScale;
                    scaledIndicator.transform.localScale = localScale;
                    calcValue = config.scaledValue*DragScale;
                    calcValueVector = CurrentValue*DragDirection;
                }
                
            }
            CurrentValueVector = Vector3.Lerp(CurrentValueVector, calcValueVector, Time.deltaTime * config.responseFactor);
            CurrentValue = Mathf.Lerp(CurrentValue, calcValue, Time.deltaTime * config.responseFactor);
        }
    }
}
