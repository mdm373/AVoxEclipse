using SSGCore.Utility;
using SSGHud;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.BlockSelection {
    public class BlockSelectionHudItem : QuickLoadItem, PuzInteractable {

        [SerializeField] private Vector2 offsetRotation;
        [SerializeField] private Transform iconAnchor;
        [SerializeField] public PuzInteractableStateConfig interactionConfig;

        private BlockModelingController modelingController;
        private GameObject icon;
        private bool isInteractionEnabled;
        private PuzInteractableStateManager stateManager;
        private VoxelBlockVolume iconWorld;
        private ushort activeBlockCode;

        public Vector2 OffsetRotation { get { return offsetRotation; } set { offsetRotation = value; } }
        public Transform IconAnchor { get { return iconAnchor; } set { iconAnchor = value; } }
        public PuzInteractableStateConfig InteractionConfig { get { return interactionConfig; } set { interactionConfig = value; } }
        
        public override void Load() {
            stateManager = new PuzInteractableStateManager(interactionConfig, this);
            stateManager.Load();
            ShowHudItem();
            InitModelingController();
            AllocateIcon();
            UpdateForBlockChange();
        }

        public override void Unload() {
            if (stateManager != null) {
                stateManager.Unload();
            }
        }

        private void AllocateIcon() {
            icon = Instantiate(modelingController.IconPrefab);
            TransformUtility.ChildAndNormalize(iconAnchor, icon.transform);
            iconWorld = icon.GetComponentInChildren<VoxelWorldComp>().World;
        }

        private void UpdateForBlockChange() {
            activeBlockCode = modelingController.ActiveBlockType;
            ModelingUtil.FillWithCode(iconWorld, activeBlockCode);
        }

        private void InitModelingController() {
            modelingController = BlockModelingController.GetSceneLoadInstance();
            modelingController.OnActiveBlockTypeChange -= HandleActiveBlockTypeChange;
            modelingController.OnActiveBlockTypeChange += HandleActiveBlockTypeChange;
        }

        private void ShowHudItem() {
            HudController.GetSceneInstance().ShowHudItem(gameObject, offsetRotation);
        }

        private void HandleActiveBlockTypeChange(ushort code) {
            UpdateForBlockChange();
        }


        public bool IsInteractionEnabled {
            get {
                return isInteractionEnabled;
            }
            set {
                isInteractionEnabled = value;
                gameObject.SetActive(isInteractionEnabled);
            }
        }

        public void HaultInteraction() { }

        public void ResetInteraction() { }
    }
}
