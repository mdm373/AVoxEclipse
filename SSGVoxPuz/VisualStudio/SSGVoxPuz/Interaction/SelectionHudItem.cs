using SSGHud;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzGlobal.PuzFont;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.Interaction {
    public class SelectionHudItem : SceneSingletonQuickLoadItem<SelectionHudItem>, PuzInteractable {

        public VoxelBlockType HoverType { set; get; }
        public VoxelWorldPosition HoverPosition { set; get; }

        public PuzFontType fontType = PuzFontType.Standard;
        public Text locationText;
        public Text typeText;
        public Text locationLabel;
        public Text typeLabel;
        public Vector2 offset;
        public PuzInteractableStateConfig interactionConfig;

        private SelectionControllerComp selectionController;
        private PuzInteractableStateManager stateManager;
        
        private bool isInteractionEnabled = true;
        private const string POSITION_FORMAT = "X[{0}] Y[{1}] Z[{2}]";
        
        public override void Load() {
            Font activeFont = PuzFontController.GetSceneLoadInstance().GetFont(fontType);
            locationText.font = activeFont;
            typeText.font = activeFont;
            locationLabel.font = activeFont;
            typeLabel.font = activeFont;
            HudController.GetSceneInstance().ShowHudItem(gameObject, offset);
            selectionController = SelectionControllerComp.GetSceneLoadInstance();
            selectionController.OnSelectionChange += HandleHoverInfoChange;
            HandleHoverInfoChange(selectionController);
            stateManager = new PuzInteractableStateManager(interactionConfig, this);
            stateManager.Load();
        }

        public override void Unload() {
            selectionController.OnSelectionChange -= HandleHoverInfoChange;
            if (stateManager != null) {
                stateManager.Unload();
            }
        }

        private void HandleHoverInfoChange(SelectionFace aSelectionController) {
            string locationContent = string.Empty;
            string typeContent = string.Empty;
            VoxelWorldPosition position = selectionController.CurrentSelectionPosition;
            if (position != VoxelWorldPosition.UNKNOWN) {
                locationContent = string.Format(POSITION_FORMAT, position.X, position.Y, position.Z);
            }
            VoxelBlockType currentType = selectionController.CurrentSelectionType;
            if (currentType != null) {
                typeContent = VoxelBlockRegistry.GetBlock(currentType).BlockName;
            }
            locationText.text = locationContent;
            typeText.text = typeContent;

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
