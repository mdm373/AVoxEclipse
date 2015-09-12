using System;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.BlockBounds;
using SSGVoxPuz.BlockSelection;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzInput;
using SSGVoxPuz.Tools;
using UnityEngine;

namespace SSGVoxPuz.Interaction {
    public class SelectionControllerComp : SceneSingletonQuickLoadItem<SelectionControllerComp>, SelectionFace, CustomUpdater, PuzInteractable, QueueItemHandler {

        public event Action<SelectionFace> OnSelectionChange;

        public float maxDistance;
        public float minDistance;
        public GameObject hitIndicatorPrefab;
        
        public PuzButton selectionButton;
        public PuzInteractableStateConfig interactionStateConfig;
        
        public Transform Gesture { get;  private set; }

        private PuzInteractableStateManager stateManager;
        private readonly TransformQueue gestureQueue = new TransformQueue();
        private bool IsSelectionRequested { get; set; }
        private Transform rootTransform;
        private PuzToolController toolController;
        private BlockModelingController modelingController;
        private bool isInsteractionEnabled;
        private HitIndicatorController hitIndcController;

        public QuickLoadItem hudItemPrefab;
        private QuickLoadItem hudItem;
        private bool selectionEnabled;

        public override void Load() {
            selectionEnabled = true;
            PuzController.GetSceneLoadInstance().Faces.Selection = this;
            GameObject hudItemObject = Instantiate(hudItemPrefab.gameObject);
            hudItem = hudItemObject.GetComponentInChildren<QuickLoadItem>();
            hudItem.Load();
            InteractionGlobalControllerComp.GetSceneLoadInstance().AddController(this);

            gestureQueue.OnQueueFrontChange -= HandleActiveGestureChange;
            gestureQueue.OnQueueFrontChange += HandleActiveGestureChange;
            stateManager = new PuzInteractableStateManager(interactionStateConfig, this);
            stateManager.Load();

            toolController = PuzToolController.GetSceneLoadInstance();
            modelingController = BlockModelingController.GetSceneLoadInstance();
            Cursor.visible = false;
            SceneCustomDelegator.AddUpdater(this);
            PuzButtonController.AddListener(selectionButton, PuzButtonDriverType.ShortPress, HandleSelected);
            rootTransform = InteractorSelectorRootComp.GetSceneInstance().transform;
            hitIndcController = new HitIndicatorController(toolController, this);
            
        }


        private void HandleSelected(PuzButtonEventData eventData) {
            if (eventData.eventType == PuzButtonEventType.PressConfirmed) {
                toolController.HandlePress();
                if (hitIndcController.IsHovering) {
                    IsSelectionRequested = true;
                }
            }
        }


        private void HandleActiveGestureChange(TransformQueue queue, Transform queueHead) {
            Gesture = queueHead;
            toolController.HandleSelectionCanceled();
            toolController.SetGesture(queueHead);
        }

        public override void Unload() {
            gestureQueue.OnQueueFrontChange -= HandleActiveGestureChange;
            stateManager.Unload();
            hitIndcController.OnDestroy();
            hudItem.Unload();
            DestroyUtility.DestroyAsNeeded(hudItem.gameObject);
        }

        public void DoUpdate() {
            if (selectionEnabled) {
                BoundsHit hit = null;
                if (Gesture != null && IsInteractionEnabled) {
                    Vector3 direction = Gesture.position - rootTransform.position;
                    Vector3 origin = rootTransform.position;
                    Ray pointRay = new Ray(origin, direction);
                    Debug.DrawLine(pointRay.origin, pointRay.direction*10000, Color.green);
                    hit = VoxelBlockBoundsCollisionUtil.GetHit(pointRay, maxDistance);
                    if (hit != null && hit.IsIntersected) {
                        hitIndcController.HandlePossibleHoverStart(hit);
                        hitIndcController.DoHoverUpdate(hit);
                        if (IsSelectionRequested) {
                            IsSelectionRequested = false;
                            ushort activeBlockType = modelingController.ActiveBlockType;
                            toolController.HandleToolRequested(hit, activeBlockType);
                        }
                    }
                    else {
                        hitIndcController.HandlePossibleHoverStop();
                        if (IsSelectionRequested) {
                            toolController.HandleSelectionCanceled();
                        }
                        IsSelectionRequested = false;
                    }
                }
                else {
                    hitIndcController.HandlePossibleHoverStop();
                }
                HandlePossibleHoverChange(hit);
            }
            else {
                hitIndcController.HandlePossibleHoverStop();
            }
        }

        public VoxelBlockType CurrentSelectionType { get; private set; }
        public VoxelWorldPosition CurrentSelectionPosition { get; private set; }

        private void HandlePossibleHoverChange(BoundsHit hit) {
            VoxelWorldPosition nowPosition = VoxelWorldPosition.UNKNOWN;
            VoxelBlockType nowType = null;
            if (hit != null && hit.IsIntersected) {
                nowPosition = hit.WorldPosition;
                nowType = hit.BlockType;
            }
            bool isChanged = nowType != CurrentSelectionType || nowPosition != CurrentSelectionPosition;
            CurrentSelectionPosition = nowPosition;
            CurrentSelectionType = nowType;
            if (isChanged && OnSelectionChange != null) {
                OnSelectionChange(this);
            }
        }
         
        public bool IsInteractionEnabled {
            get {
                return isInsteractionEnabled;
            }
            set {
                isInsteractionEnabled = value;
                if (!IsInteractionEnabled) {
                    toolController.HandleSelectionCanceled();
                }
            }
        }

        public void HaultInteraction() {
            toolController.HandleSelectionCanceled();
        }

        public void ResetInteraction() {
            IsInteractionEnabled = true;
            toolController.HandleSelectionCanceled();
        }

        public void EnqueueItem(Transform gesture) {
            gestureQueue.Enqueue(gesture);
        }

        public void DequeueItem(Transform gesture) {
            gestureQueue.Dequeue(gesture);
        }

        public void EnableSelection() {
            hudItem.gameObject.SetActive(true);
            selectionEnabled = true;
            PuzController.GetSceneLoadInstance().GetHandController().gameObject.SetActive(true);
        }

        public void DisableSelection() {
            selectionEnabled = false;
            hudItem.gameObject.SetActive(false);
            PuzController.GetSceneLoadInstance().GetHandController().gameObject.SetActive(false);
        }
    }
}
