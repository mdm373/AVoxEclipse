using SSGCore.Custom;
using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    class PuzToolHudIndicator : QuickLoadItem, PuzInteractable {

        [SerializeField]private Vector2 offsetRotation = Vector2.zero;
        [SerializeField] private Transform iconAnchorPoint;
        [SerializeField] private GameObject backgroundPrefab;
        [SerializeField] private Vector3 iconOffset;
        [SerializeField] private PuzInteractableStateConfig interactionConfig;
        private PuzToolController controller;
        private GameObject icon;
        private bool isInteractionEnabled = true;
        private PuzInteractableStateManager stateManager;


        public Vector2 OffsetRotation { get { return offsetRotation;} set { offsetRotation = value; } }
        public Vector3 IconOffset { get { return iconOffset; } set { iconOffset = value; } }
        public Transform IconAnchorPoint { get { return iconAnchorPoint; } set { iconAnchorPoint = value; } }
        public GameObject BackgroundPrefab { get { return backgroundPrefab; } set { backgroundPrefab = value; } }
        public PuzInteractableStateConfig InteractionConfig { get { return interactionConfig; } set { interactionConfig = value; } }
        
        public override void Load() {
            stateManager = new PuzInteractableStateManager(interactionConfig, this);
            AllocateBackground();
            AllocateController();
            AddToHud();
            InitActiveItem();
            stateManager.Load();
        }

        public override void Unload() {            
            stateManager.Unload();
        }

        private void AllocateBackground() {
            GameObject background = Instantiate(BackgroundPrefab);
            TransformUtility.ChildAndNormalize(transform, background.transform);
        }

        private void AllocateController() {
            controller = PuzToolController.GetSceneLoadInstance();
            controller.OnActiveItemChange -= HandleActiveItemChange;
            controller.OnActiveItemChange += HandleActiveItemChange;
        }

        private void HandleActiveItemChange(PuzTool item) {
            InitActiveItem();
        }

        private void InitActiveItem() {
            if (!CompUtil.IsNull(icon)) {
                DestroyUtility.DestroyAsNeeded(icon);
            }
            PuzTool activeTool = controller.ActiveTool;
            GameObject iconPrefab = controller.GetToolIconPrefab(activeTool);
            icon = Instantiate(iconPrefab);
            icon.name = "hud-icon";
            TransformUtility.ChildAndNormalize(iconAnchorPoint, icon.transform);
            icon.transform.localPosition = iconOffset;
        }

        private void AddToHud() {
            HudController.GetSceneInstance().ShowHudItem(gameObject, offsetRotation);
        }


        public bool IsInteractionEnabled {
            get {
                return isInteractionEnabled;
            }
            set {
                isInteractionEnabled = value;
                gameObject.SetActive(IsInteractionEnabled);
            }
        }

        public void HaultInteraction() { }

        public void ResetInteraction() { }

    }
}
