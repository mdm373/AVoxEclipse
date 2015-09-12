using SSGCore.Custom;
using UnityEngine;

#pragma warning disable 649
#pragma warning disable 169

namespace SSGVoxPuz.PuzMenu {
    public abstract class PuzMenuOptionHandlerComp : CustomBehaviour {

        [SerializeField] private GameObject iconPrefab;
        [SerializeField] private string hoverText;
        [SerializeField] private PuzMenuOptionType optionType;
        [SerializeField] private string optionId;
        [SerializeField] public bool isSelectable = true;

        public abstract void HandleOptionSelect();
        public abstract void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp);
        public GameObject IconPrefab { get { return iconPrefab; } set { iconPrefab = value; } }
        public string HoverText {
            get { return hoverText;}
            set { hoverText = value; }
        }

        public PuzMenuOptionType OptionType {
            get { return optionType; }
            set { optionType = value; }
        }

        public string OptionId { get { return optionId; } }
    }
}
