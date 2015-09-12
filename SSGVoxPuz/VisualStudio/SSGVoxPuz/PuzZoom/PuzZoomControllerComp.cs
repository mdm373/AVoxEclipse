using SSGCore.Custom;
using SSGVoxel.BlockBounds;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzZoom {
    public class PuzZoomControllerComp : SceneSingletonQuickLoadItem<PuzZoomControllerComp>, PuzInteractable, CustomUpdater, ZoomFace {

        public PuzButton zoomButton;
        public Transform toZoom;
        public Transform toLook;
        public Transform zoomRoot;
        public Transform zoomDirection;
        public float defaultZoomLevel;
        private float zoomLevel;
        public float maxZoom = 100;
        public float minZoom;
        public float zoomSpeed;
        public float zoomFactor;
        public float zoomBuffer;
        public PuzInteractableStateConfig interactionStateConfig;
        private PuzInteractableStateManager stateManager;
        private float zoomRequest;
        private float overrideZoomRequest;
        private float currentZoomSpeed;
        private InteractionGlobalControllerComp globalController;
        private bool isZoomEnabled = true;

        public override void Load() {
            PuzController.GetSceneLoadInstance().Faces.Zoom = this;
            globalController = InteractionGlobalControllerComp.GetSceneLoadInstance();
            globalController.AddController(this);
            SceneCustomDelegator.AddUpdater(this);
            PuzButtonController.AddListener(zoomButton, PuzButtonDriverType.Continue, HandleZoomContinue);
            PuzButtonController.AddListener(zoomButton, PuzButtonDriverType.Up, HandleZoomStop);
            stateManager = new PuzInteractableStateManager(interactionStateConfig, this);
            stateManager.Load();
            ResetZoomLevel();
            PositionToZoom();
        }

        public override void Unload() {
            PuzButtonController.RemoveListener(zoomButton, PuzButtonDriverType.Continue , HandleZoomContinue);
            PuzButtonController.RemoveListener(zoomButton, PuzButtonDriverType.Up, HandleZoomStop);
            stateManager.Unload();
        }

        private void ResetZoomLevel() {
            zoomLevel = defaultZoomLevel;
        }

        public void OnDestroy() {
            PuzButtonController.RemoveListener(zoomButton, PuzButtonDriverType.Continue, HandleZoomContinue);
            PuzButtonController.RemoveListener(zoomButton, PuzButtonDriverType.Up, HandleZoomStop);
            stateManager.Unload();
        }


        private void HandleZoomStop(PuzButtonEventData eventdata) {
            if (IsInteractionEnabled) {
                globalController.EnableOthers(this);
                zoomRequest = 0.0f;   
            }
        }

        private void HandleZoomContinue(PuzButtonEventData eventData) {
            if (IsInteractionEnabled && isZoomEnabled) {
                globalController.HaultOthers(this);
                globalController.DisableOthers(this);
                zoomRequest = eventData.axisDirection;
                zoomRequest = Mathf.Sign(zoomRequest);
            }
        }


        private const int MAX_FORWARD_CHECK = 1000;

        private void UpateForInsideModel() {
            Vector3 origin = toLook.position;
            Vector3 direction = toLook.forward;
            origin = origin - direction*MAX_FORWARD_CHECK;
            Ray ray = new Ray(origin, direction);
            BoundsHit hit = VoxelBlockBoundsCollisionUtil.GetHit(ray, 0, MAX_FORWARD_CHECK * 2, 0.0f);
            if (hit != null && hit.IsIntersected) {
                float distance = hit.Distance;
                overrideZoomRequest = (distance - zoomBuffer) < MAX_FORWARD_CHECK ? 1 : zoomRequest;
            }
            else {
                overrideZoomRequest = zoomRequest;
            }
        }

        private void PositionToZoom() {
            Vector3 direction = zoomDirection.position - zoomRoot.position;
            direction.Normalize();
            Vector3 zoomScale = Vector3.one * zoomLevel;
            direction.Scale(zoomScale);
            direction.Scale(toZoom.lossyScale);
            toZoom.position = zoomRoot.position + direction;
            toLook.LookAt(zoomRoot);
        }

        private bool UpdateZoomLevel() {
            float oldZoomLevel = zoomLevel;
            currentZoomSpeed = Mathf.Lerp(currentZoomSpeed, (zoomSpeed * overrideZoomRequest), Time.deltaTime * zoomFactor);
            zoomLevel = zoomLevel + currentZoomSpeed;
            if (zoomLevel > maxZoom) {
                zoomLevel = maxZoom;
            }
            if (zoomLevel < minZoom) {
                zoomLevel = minZoom;
            }
            return Mathf.Abs(oldZoomLevel  - zoomLevel) > .0001f;
        }

        public bool IsInteractionEnabled { get; set; }
        
        public void HaultInteraction() {
            currentZoomSpeed = 0.0f;
            zoomRequest = 0.0f;
        }

        public void ResetInteraction() {
            HaultInteraction();
            ResetZoomLevel();
            PositionToZoom();
        }

        public void DoUpdate() {
            UpateForInsideModel();
            if (IsInteractionEnabled && isZoomEnabled) {
                if (UpdateZoomLevel()) {
                    PositionToZoom();
                }
            }
        }

        public void EnableZoom() {
            isZoomEnabled = true;
        }

        public void DisableZoom() {
            isZoomEnabled = false;
        }

        public float GetZoomLevel() {
            return zoomLevel;
        }

        public void SetZoomLevel(float aZoomLevel) {
            zoomLevel = aZoomLevel;
            PositionToZoom();
        }

        public void HaultActiveZooming() {
            HaultInteraction();
            PositionToZoom();
        }
    }
}
