using System;
using System.Collections.Generic;
using SSGCore.Utility;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Pivot {

    [ExecuteInEditMode]
    public class PivotGuidesControllerComp : SceneSingletonQuickLoadItem<PivotGuidesControllerComp>, PuzInteractable {

        public event Action<PivotGuidesControllerComp> OnGuideShowChange;

        private const float COMP_SCALE = .0001f;

        public GameObject linePrefab = null;
        public float distance;
        public Material guideMaterial;
        public Material guideOverlayMaterial;
        public string overlayLayerName = "default";
        
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        [SerializeField, HideInInspector] private List<MeshRenderer> lines = new List<MeshRenderer>();
        [SerializeField, HideInInspector] private Transform myTransform;
        [SerializeField, HideInInspector] private MeshRenderer forwardObject;
        [SerializeField, HideInInspector] private MeshRenderer upObject;
        [SerializeField, HideInInspector] private MeshRenderer rightObject;
        [SerializeField, HideInInspector] private MeshRenderer forwardOverlayObject;
        [SerializeField, HideInInspector] private MeshRenderer upOverlayObject;
        [SerializeField, HideInInspector] private MeshRenderer rightOverlayObject;

        [SerializeField, HideInInspector] private float oldDistance = int.MinValue;
        [SerializeField, HideInInspector] private Material oldGuideOverlayMaterial;
        [SerializeField, HideInInspector] private Material oldGuideMaterial;
        [SerializeField, HideInInspector] private int overlayLayer;
        [SerializeField, HideInInspector] private string oldOverlayLayerName = string.Empty;
        [SerializeField, HideInInspector] private int oldLayer = int.MinValue;
        private PuzMenuControllerComp menuControllerComp;
        private bool isGuideShown = true;
        private bool isGuidesSuppressed;

        public void OnEnable() {
            myTransform = transform;
        }

        private void HandleMenuShow(PuzMenuControllerComp obj) {
            isGuidesSuppressed = true;
            UpdateGuidesShown();
        }

        private void HandleMenuHide(PuzMenuControllerComp obj) {
            isGuidesSuppressed = false;
            UpdateGuidesShown();
        }

        public void OnRenderObject() {
            AllocateLine(ref forwardObject, Vector3.forward, guideMaterial, gameObject.layer);
            AllocateLine(ref upObject, Vector3.up, guideMaterial, gameObject.layer);
            AllocateLine(ref rightObject, Vector3.right, guideMaterial, gameObject.layer);
            AllocateLine(ref rightOverlayObject, Vector3.right, guideOverlayMaterial, overlayLayer);
            AllocateLine(ref upOverlayObject, Vector3.up, guideOverlayMaterial, overlayLayer);
            AllocateLine(ref forwardOverlayObject, Vector3.forward, guideOverlayMaterial, overlayLayer);
            if (Mathf.Abs(oldDistance - distance) > COMP_SCALE) {
                RescaleLines();
                oldDistance = distance;
            }
            if (guideOverlayMaterial != oldGuideOverlayMaterial) {
                ReMaterialOveralys();
                oldGuideOverlayMaterial = guideOverlayMaterial;
            }
            if (guideMaterial != oldGuideMaterial) {
                ReMaterialGuides();
                oldGuideMaterial = guideMaterial;
            }
            if (!oldOverlayLayerName.Equals(overlayLayerName)) {
                overlayLayer = LayerMask.NameToLayer(overlayLayerName);
                ReLayerOverlays();
                oldOverlayLayerName = overlayLayerName;
            }
            if (oldLayer != gameObject.layer) {
                ReLayerGuides();
                oldLayer = gameObject.layer;
            }

        }

        private void ReLayerGuides() {
            upObject.gameObject.layer = gameObject.layer;
            rightObject.gameObject.layer = gameObject.layer;
            forwardObject.gameObject.layer = gameObject.layer;
        }

        private void ReLayerOverlays() {
            upOverlayObject.gameObject.layer = overlayLayer;
            rightOverlayObject.gameObject.layer = overlayLayer;
            forwardOverlayObject.gameObject.layer = overlayLayer;
        }

        private void ReMaterialGuides() {
            forwardObject.sharedMaterial = guideMaterial;
            upObject.sharedMaterial = guideMaterial;
            rightObject.sharedMaterial = guideMaterial;
        }

        private void ReMaterialOveralys() {
            forwardOverlayObject.sharedMaterial = guideOverlayMaterial;
            upOverlayObject.sharedMaterial = guideOverlayMaterial;
            rightOverlayObject.sharedMaterial = guideOverlayMaterial;
        }

        private void RescaleLines() {
            for (int i = 0; i < lines.Count; i++) {
                if (CompUtil.IsNull(lines[i])) {
                    lines.RemoveAt(i);
                    i--;
                }
                else {
                    RescaleLine(lines[i].gameObject);
                }
            }
        }

        private void AllocateLine(ref MeshRenderer line, Vector3 direction, Material material, int layer) {
            if (line == null) {
                GameObject lineObject = Instantiate(linePrefab);
                lineObject.name = "guide-line";
                lineObject.layer = layer;
                TransformUtility.ChildAndNormalize(myTransform, lineObject.transform);
                line = lineObject.GetComponent<MeshRenderer>();
                line.sharedMaterial = material;
                line.transform.forward = direction;
                lines.Add(line);
                RescaleLine(lineObject);
            }
        }

        private void RescaleLine(GameObject line) {
            Vector3 calcScale = new Vector3(1.0f, 1.0f, distance);
            line.transform.localScale = calcScale;
        }

        public void SetGuidesShown(bool isShown) {
            bool wasGuidesShown = isGuideShown;
            isGuideShown = isShown;
            UpdateGuidesShown();
            if (OnGuideShowChange != null && wasGuidesShown != isGuideShown) {
                OnGuideShowChange(this);
            }
        }

        public bool IsGuidesShown() {
            return isGuideShown;
        }

        public bool IsInteractionEnabled { get; set; }
        public void HaultInteraction() { }

        public void ResetInteraction() {
            SetGuidesShown(true);
        }

        private void UpdateGuidesShown() {
            bool isGuidesVisible = isGuideShown && !isGuidesSuppressed;
            for (int i = 0; i < lines.Count; i++) {
                lines[i].gameObject.SetActive(isGuidesVisible);
            }
        }

        public override void Load() {
            InteractionGlobalControllerComp.GetSceneLoadInstance().AddController(this);
            menuControllerComp = PuzMenuControllerComp.GetSceneLoadInstance();
            menuControllerComp.OnHide += HandleMenuHide;
            menuControllerComp.OnShow += HandleMenuShow;
        }

        public override void Unload() {
            menuControllerComp.OnHide -= HandleMenuHide;
            menuControllerComp.OnShow -= HandleMenuShow;
        }
    }
}
