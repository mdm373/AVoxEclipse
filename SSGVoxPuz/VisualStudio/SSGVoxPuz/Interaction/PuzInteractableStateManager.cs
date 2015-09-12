using System;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Interaction {

    [Serializable]
    public class PuzInteractableStateConfig {
        public bool enableWithMenu = false;
        public bool enableWithNoMenu = true;
    }

    public class PuzInteractableStateManager {


        private readonly PuzInteractable interactable;
        private readonly PuzInteractableStateConfig config;
        private PuzMenuControllerComp menu;


        public PuzInteractableStateManager(PuzInteractableStateConfig aConfig, PuzInteractable aInteractable) {
            config = aConfig;
            interactable = aInteractable;
        }

        public void Load() {
            if (menu != null) {
                menu.OnShow -= HandleMenuShow;
                menu.OnHide -= HandleMenuHide;
            }
            menu = PuzMenuControllerComp.GetSceneLoadInstance();
            if (menu != null) {
                menu.OnShow -= HandleMenuShow;
                menu.OnShow += HandleMenuShow;
                menu.OnHide -= HandleMenuHide;
                menu.OnHide += HandleMenuHide;

                bool isMenuShown = menu.IsShown;
                interactable.IsInteractionEnabled = isMenuShown ? config.enableWithMenu : config.enableWithNoMenu;
            }
        }

        public void Unload() {
            if (menu != null) {
                menu.OnHide -= HandleMenuHide;
                menu.OnShow -= HandleMenuShow;
            }
        }

        private void HandleMenuHide(PuzMenuControllerComp aMenu) {
            interactable.IsInteractionEnabled = config.enableWithNoMenu;
        }

        private void HandleMenuShow(PuzMenuControllerComp aMenu) {
            interactable.IsInteractionEnabled = config.enableWithMenu;
        }
    }
}
