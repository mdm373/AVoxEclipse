using System.Collections.Generic;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    public class PuzMenuSelectorComp : CustomBehaviour, CustomUpdater {
        
        private PuzMenuSelectionConfig selectionConfig;
        private PuzMenuControllerComp controllerComp;
        private PuzMenuInteractable aInteraction;

        private Transform myTransform;
        private bool IsMenuInteractionEnabled { get; set; }

        public List<GameObject> selectionShow;
        public List<GameObject> selectionHide;
        private Transform cameraRoot;

        public void OnEnable(){
            myTransform = transform;
            SceneCustomDelegator.AddUpdater(this);
            controllerComp = PuzMenuControllerComp.GetSceneLoadInstance();
            if (controllerComp != null) {
                controllerComp.OnSelectionEnabled -= HandleSelectionEnabled;
                controllerComp.OnSelectionEnabled += HandleSelectionEnabled;
                controllerComp.OnSelectionDisabled -= HandleSelectionDisabled;
                controllerComp.OnSelectionDisabled += HandleSelectionDisabled;
                if (controllerComp.IsMenuInteractionEnabled) {
                    SetUpForSelection(controllerComp);
                }
                else {
                    HandleSelectionDisabled(controllerComp);
                }
            }

        }

        private void HandleSelectionDisabled(PuzMenuControllerComp menu) {
            IsMenuInteractionEnabled = false;
            UpdateSelectionDisplay();
            StopListeningToButtons();
        }

        private void HandleSelectionEnabled(PuzMenuControllerComp menu) {
            SetUpForSelection(menu);
        }

        private void SetUpForSelection(PuzMenuControllerComp menu) {
            IsMenuInteractionEnabled = true;
            cameraRoot = menu.CameraRoot;
            selectionConfig = menu.selectionConfig;
            UpdateSelectionDisplay();
            StopListeningToButtons();
            if (selectionConfig != null) {
                PuzButtonController.AddListener(selectionConfig.selectionButton, PuzButtonDriverType.ShortPress, HandleOptionSelected);
                PuzButtonController.AddListener(selectionConfig.alternateSelectionButton, PuzButtonDriverType.ShortPress, HandleOptionSelected);
            }
        }

        public void OnDestroy() {
            if (controllerComp != null) {
                controllerComp.OnSelectionEnabled -= HandleSelectionEnabled;
                controllerComp.OnSelectionDisabled -= HandleSelectionDisabled;
            }
            StopListeningToButtons();
            if (!CompUtil.IsNull(aInteraction)) {
                aInteraction.HandleHoverEnd();
            }
        }

        private void StopListeningToButtons() {
            if (selectionConfig != null) {
                PuzButtonController.RemoveListener(selectionConfig.selectionButton, PuzButtonDriverType.ShortPress, HandleOptionSelected);
                PuzButtonController.RemoveListener(selectionConfig.alternateSelectionButton, PuzButtonDriverType.ShortPress, HandleOptionSelected);
            }
        }

        private void HandleOptionSelected(PuzButtonEventData eventData) {
            if (eventData.eventType == PuzButtonEventType.PressConfirmed) {
                if (aInteraction != null) {
                    aInteraction.HandleSelected();
                }
                else {
                    PuzMenuControllerComp.GetSceneLoadInstance().HideMenu();
                }   
            }
        }

        public void DoUpdate() {
            if (IsMenuInteractionEnabled) {
                RaycastHit hitInfo;
                Ray ray = GetSelectionRay();
                bool isHit =Physics.Raycast(ray, out hitInfo, selectionConfig.maxDistance, selectionConfig.selectionLayerMask);
                Debug.DrawLine(ray.origin, ray.origin + (ray.direction * selectionConfig.maxDistance));
                if (isHit) {
                    PuzMenuOptionColliderComp currentCollider = hitInfo.collider.GetComponent<PuzMenuOptionColliderComp>();
                    if (currentCollider != null) {
                        PuzMenuInteractable interaction = currentCollider.Interaction;
                        if (aInteraction != null) {
                            if (interaction != aInteraction) {
                                aInteraction.HandleHoverEnd();
                                interaction.HandleHoverStart();
                            }
                        }
                        else {
                            interaction.HandleHoverStart();
                        }
                        aInteraction = interaction;
                    }
                }
                else {
                    if (aInteraction != null) {
                        aInteraction.HandleHoverEnd();
                        aInteraction = null;
                    }
                }
            }
        }

        private Ray GetSelectionRay() {
            Vector3 direction = myTransform.position - cameraRoot.position;
            direction.Normalize();
            Vector3 origin = cameraRoot.position;
            return new Ray(origin, direction);
        }


        private void UpdateSelectionDisplay() {
            for (int i = 0; i < selectionShow.Count; i++) {
                selectionShow[i].SetActive(IsMenuInteractionEnabled);
            }
            for (int i = 0; i < selectionHide.Count; i++) {
                selectionHide[i].SetActive(!IsMenuInteractionEnabled);
            }
        }
    }
}
