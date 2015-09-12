using System.Collections.Generic;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Interaction {
    public class InteractionGlobalControllerComp : SceneSingletonQuickLoadItem<InteractionGlobalControllerComp> {

        private readonly List<PuzInteractable> interactables = new List<PuzInteractable>();
        private PuzController puzController;

        public override void Load() {
            if (puzController == null) {
                puzController = PuzController.GetSceneLoadInstance();
                puzController.OnReset += HandleReset;
            }

        }

        public override void Unload() {
            if (!CompUtil.IsNull(puzController)) {
                puzController.OnReset -= HandleReset;
            }
        }

        private void HandleReset(PuzController obj) {
            ResetAll();
        }

        public void ResetAll() {
            for (int i = 0; i < interactables.Count; i++) {
                interactables[i].IsInteractionEnabled = true;
                interactables[i].ResetInteraction();
            }
        }
        
        public void AddController(PuzInteractable interactable) {
            if (!interactables.Contains(interactable)) {
                interactables.Add(interactable);
            }
        }

        public void HaultOthers(PuzInteractable requesting) {
            for (int i = 0; i < interactables.Count; i++) {
                if (interactables[i] != requesting) {
                    interactables[i].HaultInteraction();
                }
            }
        }

        public void EnableOthers(PuzInteractable requesting) {
            for (int i = 0; i < interactables.Count; i++) {
                if (interactables[i] != requesting) {
                    interactables[i].IsInteractionEnabled = true;
                }
            }
        }

        public void DisableOthers(PuzInteractable requesting) {
            for (int i = 0; i < interactables.Count; i++) {
                if (interactables[i] != requesting) {
                    interactables[i].IsInteractionEnabled = false;
                }
            }
        }
    }
}
