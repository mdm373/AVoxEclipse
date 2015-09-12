using System.Collections.Generic;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.BlockBounds;
using SSGVoxPuz.Tools;
using UnityEngine;

namespace SSGVoxPuz.Interaction {
    class HitIndicatorController {
        private readonly PuzToolController toolController;
        private readonly Dictionary<PuzTool, GameObject> indicatorPrefabMap;
        private readonly Dictionary<VoxelDirection, Vector3> rotationMap;
        private GameObject activeIndicator;
        private readonly CustomBehaviour parent;
        private VoxelBlockVolume activeWorld;
        private VoxelDirection oldFacing;
        private VoxelWorldPosition oldPosition;
        private Animator anim;

        public bool IsHovering { get; private set; }

        public HitIndicatorController(PuzToolController aToolController, CustomBehaviour aParent) {
            parent = aParent;
            toolController = aToolController;
            toolController.OnActiveItemChange += HandleToolChanged;
            indicatorPrefabMap = BuildIndicatorPrefabMap(toolController, parent);
            activeIndicator = indicatorPrefabMap[toolController.ActiveTool];
            rotationMap = BuildRotationMap();
        }

        private static Dictionary<VoxelDirection, Vector3> BuildRotationMap() {
            Dictionary<VoxelDirection, Vector3> rotMap = new Dictionary<VoxelDirection, Vector3>();
            rotMap[VoxelDirection.South] = new Vector3(0.0f, 180f, 0.0f);
            rotMap[VoxelDirection.North] = new Vector3(0.0f, 0.0f, 0.0f);
            rotMap[VoxelDirection.Up] = new Vector3(-90.0f, 0.0f, 0.0f);
            rotMap[VoxelDirection.Down] = new Vector3(90.0f, 0.0f, 0.0f);
            rotMap[VoxelDirection.East] = new Vector3(0.0f, 90.0f, 0.0f);
            rotMap[VoxelDirection.West] = new Vector3(0.0f, -90.0f, 0.0f);
            return rotMap;
        }

        private static Dictionary<PuzTool, GameObject> BuildIndicatorPrefabMap(PuzToolController controller, CustomBehaviour aParent) {
            List<PuzTool> activeTools = controller.GetActiveTools();
            Dictionary<PuzTool, GameObject> prefabMap = new Dictionary<PuzTool, GameObject>();
            for (int i = 0; i < activeTools.Count; i++) {
                GameObject hoverPrefab = controller.GetHoverPrefab(activeTools[i]);
                GameObject hoverIcon = Object.Instantiate(hoverPrefab);
                hoverIcon.name = "hover-icon-" + activeTools[i].ToString();
                TransformUtility.ChildAndNormalize(aParent.transform, hoverIcon.transform);
                prefabMap[activeTools[i]] = hoverIcon;
                hoverIcon.SetActive(false);
            }
            return prefabMap;
        }

        public void OnDestroy() {
            toolController.OnActiveItemChange -= HandleToolChanged;
        }

        private void HandleToolChanged(PuzTool activeTool) {
            if (activeIndicator != null) {
                DisableActiveIndicator();
            }
            activeIndicator = indicatorPrefabMap[activeTool];
            if (IsHovering) {
                EnableActiveIndicator();
            }
        }

        private void EnableActiveIndicator() {
            activeIndicator.SetActive(true);
            TransformUtility.ChildAndNormalize(activeWorld.GetTransform(), activeIndicator.transform);
            anim = activeIndicator.GetComponentInChildren<Animator>();
        }

        private void DisableActiveIndicator() {
            activeIndicator.SetActive(false);
            Transform parentTransform = parent.transform;
            Transform activeIndicatorTransform = activeIndicator.transform;
            TransformUtility.ChildAndNormalize(parentTransform, activeIndicatorTransform);
        }

        public void HandlePossibleHoverStart(BoundsHit hitInfo) {
            if (!IsHovering) {
                IsHovering = true;
                activeWorld = hitInfo.World;
                EnableActiveIndicator();
                oldFacing = VoxelDirection.Unknown;
                oldPosition = VoxelWorldPosition.UNKNOWN;
            }            
        }

        public void HandlePossibleHoverStop() {
            if (IsHovering) {
                IsHovering = false;
                activeWorld = null;
                DisableActiveIndicator();
            }
        }

        public void DoHoverUpdate(BoundsHit hitInfo) {
            if (IsHovering) {
                if (hitInfo.Face != oldFacing || hitInfo.WorldPosition != oldPosition) {
                    oldFacing = hitInfo.Face;
                    oldPosition = hitInfo.WorldPosition;
                    
                    if (anim != null) {
                        anim.SetBool("Changed", true);
                    }
                }
                else {
                    if (anim != null) {
                        anim.SetBool("Changed", false);
                    }
                }
                VoxelDirection hitDirection = hitInfo.Face;
                Vector3 rotation = rotationMap[hitDirection];
                activeIndicator.transform.localRotation = Quaternion.Euler(rotation);
                Vector3 positionVector = hitInfo.WorldPosition.GetVector();
                Vector3 offset = (hitInfo.AdjacentWorldPosition.GetVector() - positionVector);
                offset = positionVector + offset*.5f;
                activeIndicator.transform.localPosition = offset;
            }
        }
    }
}
