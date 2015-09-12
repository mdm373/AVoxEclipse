using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzMod {
    public class PuzModHudIndicator : SceneSingletonQuickLoadItem<PuzModHudIndicator>, PuzInteractable {

        public Vector2 offsetRotation;
        public GameObject backgroundPrefab;
        public Transform iconAnchorPoint;
        public GameObject symmatryEnabledIconPrefab;
        public PuzInteractableStateConfig interactionConfig;
        
        private GameObject background;
        private PuzModificationController modController;
        private GameObject symmatryIcon;
        private bool isInteractionEnabled;
        private PuzInteractableStateManager stateManager;
        


        public override void Load() {
            stateManager = new PuzInteractableStateManager(interactionConfig, this);
            AddToHud();
            AllocateController();
            AllocateBackground();
            AllocateIcon();
            UpdateIconState();
            stateManager.Load();
        }


        private void AllocateIcon() {
            symmatryIcon = Instantiate(symmatryEnabledIconPrefab);
            symmatryIcon.name = "symmatry-enabled-icon";
            TransformUtility.ChildAndNormalize(iconAnchorPoint, symmatryIcon.transform);

        }

        private void UpdateIconState() {
            symmatryIcon.SetActive(modController.IsSymmatryEnabled);
            background.SetActive(modController.IsSymmatryEnabled);
        }

        public override void Unload() {
            modController.OnSymmatryModeChange -= HandleSymmatryModeChanged;    
            stateManager.Unload();
        }

        private void AllocateBackground() {
            background = Instantiate(backgroundPrefab);
            background.name = "background";
            TransformUtility.ChildAndNormalize(transform, background.transform);
        }

        private void AllocateController() {
            modController = PuzModificationController.GetSceneLoadInstance();
            modController.OnSymmatryModeChange -= HandleSymmatryModeChanged;
            modController.OnSymmatryModeChange += HandleSymmatryModeChanged;
        }

        private void HandleSymmatryModeChanged(PuzModificationController controller) {
            UpdateIconState();
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
