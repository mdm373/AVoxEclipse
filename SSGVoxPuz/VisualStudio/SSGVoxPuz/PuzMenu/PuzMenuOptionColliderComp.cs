using SSGCore.Custom;

namespace SSGVoxPuz.PuzMenu {
    public class PuzMenuOptionColliderComp : CustomBehaviour {

        public PuzMenuInteractable Interaction { get; private set; }

        public void Init(PuzMenuInteractable option) {
            Interaction = option;
        }

    }
}
