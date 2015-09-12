using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzRotation {
    public class PuzRotationBallControllerComp : SceneSingletonQuickLoadItem<PuzRotationBallControllerComp>, 
        PuzInteractable, CustomUpdater, QueueItemHandler, RotationFace {


        [SerializeField] private PuzInteractableStateConfig interactionConfig;
        [SerializeField] private PuzButton rotationButton;
        [SerializeField] private float focalDistance = 1;

        private readonly TransformQueue transformQueue = new TransformQueue();
        private PuzInteractableStateManager interactionManager;
        private Transform activeTransform;
        private Vector3 focalPosition;
        private bool isRotating;
        private bool isRotationRequested;
        private InteractionGlobalControllerComp globalInteractionController;
        private GameObject rotationBall;
        private GameObject pointIndicator;
        private Vector3 startPosition;

        public PuzInteractableStateConfig InteractionConfig { get { return interactionConfig;} set { interactionConfig = value; } }
        public PuzButton RotationButton { get { return rotationButton; } set { rotationButton = value; } }
        public float FocalDistance { get { return focalDistance; } set { focalDistance = value; } }
        public bool IsInteractionEnabled { get; set; }

        public GameObject rotationBallPrefab;
        public GameObject pointIndicatorPrefab;
        public Transform rotated;
        public Transform rotationWrap;
        public float minFocalScale = 1.5f;
        public float rotationFactor;
        public float minMoveDistance = 1;
        private GameObject indicationRoot;
        private bool isRotatationEnabled;

        public override void Load() {
            isRotatationEnabled = true;
            PuzController.GetSceneLoadInstance().Faces.Rotate = this;
            
            indicationRoot = new GameObject("indication-root");
            TransformUtility.ChildAndNormalize(transform, indicationRoot.transform);
            rotationBall = Instantiate(rotationBallPrefab);
            pointIndicator = Instantiate(pointIndicatorPrefab);
            
            TransformUtility.ChildAndNormalize(indicationRoot.transform, pointIndicator.transform);
            TransformUtility.ChildAndNormalize(indicationRoot.transform, rotationBall.transform);
            indicationRoot.SetActive(false);
            SceneCustomDelegator.AddUpdater(this);
            interactionManager = new PuzInteractableStateManager(interactionConfig, this);
            transformQueue.OnQueueFrontChange -= HandleQueueFrontChange;
            transformQueue.OnQueueFrontChange += HandleQueueFrontChange;
            PuzButtonController.AddListener(rotationButton, PuzButtonDriverType.Down, HandleRotationStart);
            PuzButtonController.AddListener(rotationButton, PuzButtonDriverType.Up, HandleRotationStop);
            globalInteractionController = InteractionGlobalControllerComp.GetSceneLoadInstance();
            globalInteractionController.AddController(this);
            interactionManager.Load();
        }

        public override void Unload() {
            interactionManager.Unload();
            PuzButtonController.RemoveListener(rotationButton, PuzButtonDriverType.Down, HandleRotationStart);
            PuzButtonController.RemoveListener(rotationButton, PuzButtonDriverType.Up, HandleRotationStop);
        }

        private void HandleRotationStop(PuzButtonEventData eventData) {
            if (isRotatationEnabled && IsInteractionEnabled) {
                globalInteractionController.EnableOthers(this);
                StopRotation();
            }
        }

        private void StopRotation() {
            isRotationRequested = false;
            indicationRoot.SetActive(false);
            isRotating = false;
            rotationWrap.rotation = rotated.rotation;
            rotated.localRotation = Quaternion.identity;
        }

        

        private void HandleRotationStart(PuzButtonEventData eventData){
            if (isRotatationEnabled && IsInteractionEnabled) {
                if (activeTransform != null && IsInteractionEnabled) {
                    isRotationRequested = true;
                    startPosition = activeTransform.position;
                }
            }
        }

        private void StartRotation() {
            isRotating = true;
            isRotationRequested = false;
            globalInteractionController.HaultOthers(this);
            globalInteractionController.DisableOthers(this);
            Vector3 focalOffset = activeTransform.forward.normalized * (focalDistance * transform.lossyScale.magnitude);
            indicationRoot.SetActive(true);
            focalPosition = startPosition + focalOffset;
            indicationRoot.transform.position = focalPosition;


            Quaternion rotation = rotated.rotation;
            Vector3 projectedLookPosition = rotated.position +
                                                (activeTransform.position - focalPosition).normalized;

            rotationWrap.LookAt(projectedLookPosition, rotationWrap.up);
            rotated.rotation = rotation;
        }

        private void HandleQueueFrontChange(TransformQueue queue, Transform queuehead) {
            activeTransform = queuehead;
        }
        
        public void HaultInteraction() {
            StopRotation();
        }

        public void ResetInteraction() {
            StopRotation();
            rotationWrap.rotation = Quaternion.identity;
            rotated.rotation = Quaternion.identity;
        }

        public void DoUpdate() {
            HandleRotationRequest();
            HandleRotating();
        }

        private void HandleRotating() {
            if (isRotating) {
                if (activeTransform != null) {
                    Debug.DrawLine(activeTransform.position, focalPosition);
                    float distance = (activeTransform.position - focalPosition).magnitude;
                    float minDistance = FocalDistance * minFocalScale;
                    if (distance > minDistance) {
                        indicationRoot.transform.LookAt(activeTransform.position, indicationRoot.transform.up);
                        Vector3 lookForward = (activeTransform.position - focalPosition).normalized;
                        Quaternion lookRotation = Quaternion.LookRotation(lookForward, rotationWrap.up);
                        rotationWrap.rotation = Quaternion.Slerp(rotationWrap.rotation, lookRotation, Time.deltaTime * rotationFactor);
                    }
                }
                else {
                    StopRotation();
                    globalInteractionController.EnableOthers(this);
                }
            }
        }

        private void HandleRotationRequest() {
            if (isRotationRequested) {
                if (activeTransform != null) {
                    Vector3 currentPosition = activeTransform.position;
                    float distance = (currentPosition - startPosition).magnitude;
                    float minMove = (activeTransform.lossyScale*minMoveDistance).magnitude;
                    if (distance > minMove) {
                        StartRotation();
                    }
                }
                else {
                    isRotationRequested = false;
                }
            }
        }

        public void EnqueueItem(Transform item) {
            transformQueue.Enqueue(item);
        }

        public void DequeueItem(Transform item) {
            transformQueue.Dequeue(item);
        }

        public void EnableRoation() {
            isRotatationEnabled = true;
        }

        public void DisableRotation() {
            HaultInteraction();
            isRotatationEnabled = false;
        }

        public void SetRotation(Vector3 rotation) {
            HaultInteraction();
            rotationWrap.localRotation = Quaternion.Euler(rotation);
        }

        public Vector3 GetRotation() {
            HaultInteraction();
            return rotationWrap.localRotation.eulerAngles;
        }
    }
}
