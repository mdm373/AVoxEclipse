using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    public abstract class PuzToggleMenuOptionHandler : PuzMenuOptionHandlerComp {

        public GameObject mainIconPrefab;
        public GameObject altIconPrefab;
        public string altHoverText;
        public string mainHoverText;
        public bool isInitAsToggled;

        private bool isToggled;
        private GameObject mainIcon;
        private GameObject altIcon;
        private PuzMenuOptionLookup lookUp;
        
        public override void HandleOptionSelect() {
            isToggled = !isToggled;
            UpdateForToggleState();
            HandleOptionSelect(isToggled);
        }

        protected void UpdateForToggleState() {
            mainIcon.SetActive(!isToggled);
            altIcon.SetActive(isToggled);
            HoverText = (isToggled) ? altHoverText : mainHoverText;
            lookUp.ChangeSubTitle(HoverText);
        }

        protected bool IsToggled {
            get { return isToggled; }
            set { 
                isToggled = value; 
                UpdateForToggleState(); 
            }
        }

        protected void SetToggleState(bool isToggledRequest) {
            isToggled = isToggledRequest;
            UpdateForToggleState();
        }

        protected abstract void HandleOptionSelect(bool isToggled);

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup aLookUp) {
            lookUp = aLookUp;
            isToggled = isInitAsToggled;
            mainIcon = Instantiate(mainIconPrefab);
            mainIcon.name = "icon-main";
            altIcon = Instantiate(altIconPrefab);
            altIcon.name = "alt-icon";
            TransformUtility.ChildAndNormalize(icon.transform, mainIcon.transform);
            TransformUtility.ChildAndNormalize(icon.transform, altIcon.transform);
            UpdateForToggleState();
            HandleOptionInitExtended(icon, aLookUp);
        }

        protected abstract void HandleOptionInitExtended(GameObject icon, PuzMenuOptionLookup aLookUp);
    }
}
