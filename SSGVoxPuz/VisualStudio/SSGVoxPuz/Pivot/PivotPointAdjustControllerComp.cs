using System;
using SSGCore.Custom;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.Pivot {
    
    
    public class PivotPointAdjustControllerComp : SceneSingletonQuickLoadItem<PivotPointAdjustControllerComp>, 
        PuzInteractable, CustomUpdater, QueueItemHandler, PivotFace {

        private enum AdjustState {
            Active,Inactive
        }

        public PuzButton pressButton = PuzButton.ScrollClick;
        public float pressMoveThreshold = .01f;
        public PuzInteractableStateConfig interactionConfig;
        public Transform pivotBase;
        public GameObject pivotGuides;
        

        [SerializeField] private DragIndicationConfig dragConfig;
        
        private PuzInteractableStateManager interactionManager;
        private AdjustState adjustState = AdjustState.Inactive;
        private TransformQueue gestureQueue = new TransformQueue();
        private Transform activeGesture;
        private bool isInteractionHaulted;
        private Vector3 pressStartPosition;
        private readonly DragIndicationController dragController = new DragIndicationController();
        private bool isPivotingEnabled;


        public DragIndicationConfig DragConfig { get { return dragConfig; } set { dragConfig = value; } }


        public override void Load() {
            PuzController.GetSceneLoadInstance().Faces.Pivot = this;
            isPivotingEnabled = true;
            dragController.Config = dragConfig;
            InteractionGlobalControllerComp.GetSceneLoadInstance().AddController(this);
            SceneCustomDelegator.AddUpdater(this);

            interactionManager = new PuzInteractableStateManager(interactionConfig, this);
            interactionManager.Load();
            
            gestureQueue = new TransformQueue();
            gestureQueue.OnQueueFrontChange += HandleActiveGestureChange;
            
            PuzButtonController.AddListener(pressButton, PuzButtonDriverType.ShortPress, HandleAdjestClicked);
            dragController.OnLoad(this);
            
        }

        private void HandleActiveGestureChange(TransformQueue queue, Transform queuehead) {
            activeGesture = queuehead;
            if (adjustState == AdjustState.Active) {
                DisableDragging();
            }
            adjustState = AdjustState.Inactive;
        }

        public override void Unload() {
            interactionManager.Unload();
        }
        
        private void HandleAdjestClicked(PuzButtonEventData eventData) {
            if (IsInteractionEnabled && activeGesture != null && isPivotingEnabled) {
                if (eventData.eventType == PuzButtonEventType.PressPossibleStart) {
                    pressStartPosition = activeGesture.position;
                }
                else {
                    Vector3 distance = pressStartPosition - activeGesture.position;
                    float distanceDragged = distance.magnitude * (transform.lossyScale.magnitude);
                    if (distanceDragged < pressMoveThreshold || adjustState == AdjustState.Active) {
                        IncrementAdjustState();
                        if (adjustState == AdjustState.Active) {
                            InteractionGlobalControllerComp.GetSceneLoadInstance().HaultOthers(this);
                            InteractionGlobalControllerComp.GetSceneLoadInstance().DisableOthers(this);
                            isInteractionHaulted = false;
                            dragController.StartDragging(activeGesture, pressStartPosition);
                        }
                        else {
                            DisableDragging();
                        }
                    }
                }
            }
            else {
                adjustState = AdjustState.Inactive;
                pressStartPosition = Vector3.zero;
            }
        }

        private void DisableDragging() {
            InteractionGlobalControllerComp.GetSceneLoadInstance().EnableOthers(this);
            dragController.StopDragging();
        }

        private void IncrementAdjustState() {
            int max = Enum.GetValues(typeof(AdjustState)).Length;
            int current = (int) adjustState;
            int next = current + 1;
            if (next >= max) {
                next = 0;
            }
            adjustState = (AdjustState) next;
        }

        public bool IsInteractionEnabled { get; set; }

        public void HaultInteraction() {
            isInteractionHaulted = true;
            dragController.StopDragging();
        }

        public void ResetInteraction() {
            adjustState = AdjustState.Inactive;
            HaultInteraction();
            pivotBase.localPosition = Vector3.zero;
        }

        public void DoUpdate() {
            dragController.DoUpdate();
            if (!isInteractionHaulted) {
                Vector3 dragVelocity = dragController.CurrentValueVector;
                pivotBase.position = pivotBase.position + dragVelocity;
            }
        }

        public void EnqueueItem(Transform gesture) {
            gestureQueue.Enqueue(gesture);
        }

        public void DequeueItem(Transform gesture) {
            gestureQueue.Dequeue(gesture);
        }

        public void EnablePivoting() {
            isPivotingEnabled = true;
            pivotGuides.SetActive(true);
        }

        public void DisablePivoting() {
            isPivotingEnabled = false;
            HaultInteraction();
            pivotGuides.SetActive(false);
        }

        public void SetPosition(Vector3 position) {
            HaultInteraction();
            pivotBase.localPosition = position;
        }

        public Vector3 GetPosition() {
            return pivotBase.localPosition;
        }
    }
}
