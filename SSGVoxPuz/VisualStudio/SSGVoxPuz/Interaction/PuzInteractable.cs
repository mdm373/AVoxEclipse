using System;

namespace SSGVoxPuz.Interaction {
    
    public interface PuzInteractable {

        bool IsInteractionEnabled { get; set; }
        void HaultInteraction();
        void ResetInteraction();
    }
}
