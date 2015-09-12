using System.Collections.Generic;
using System.Linq;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    
    public class PuzMenuScreenComp : QuickLoadItem, PuzMenuOptionLookup {

        public PuzMenuOptionConfig menuConfig;
        public List<PuzMenuOptionTypeBinding> types;
        public GameObject optionsRoot;
        public PuzScreenLayoutConfig layout;
        
        private Transform myTransform;
        private GameObject myGameObject;
        private PuzMenuOptionHandlerComp[] menuOptions;
        private PuzMenuOptionLabelHandlerComp subTitle;
        private PuzMenuOptionComp navOption;
        private List<PuzMenuOptionComp> itemComps;

        public override void Load() {
            itemComps = new List<PuzMenuOptionComp>();
            myTransform = transform;
            myGameObject = gameObject;

            PuzDynaMenuBuilder[] dynamicBuilders = optionsRoot.GetComponentsInChildren<PuzDynaMenuBuilder>();
            for (int i = 0; i < dynamicBuilders.Count(); i++) {
                dynamicBuilders[i].BuildDynamicItems(optionsRoot, layout);
            }

            menuOptions = optionsRoot.GetComponentsInChildren<PuzMenuOptionHandlerComp>();
            for (int i = 0; i < menuOptions.Length; i++) {
                PuzMenuOptionType type = menuOptions[i].OptionType;
                GameObject menuItem = GetMenuItemForType(type);
                TransformUtility.ChildAndNormalize(myTransform, menuItem.transform);
                if (layout.layoutType == PuzScreenLayoutType.Radial) {
                    PositionForRadial(i, menuItem);
                }
                else {
                    PositionForGrid(i, menuItem);
                }
                    
                PuzMenuOptionComp itemComp = menuItem.GetComponent<PuzMenuOptionComp>();
                itemComp.Init(menuOptions[i], menuConfig, this);
                itemComps.Add(itemComp);
            }
            myGameObject.SetActive(false);
        }

        public override void Unload() {
            
        }

        private void HandleHoverEnd(PuzMenuOptionHandlerComp option) {
            subTitle.SetText(string.Empty);
        }

        private void HandleHoverStart(PuzMenuOptionHandlerComp option) {
            subTitle.SetText(option.HoverText);
        }


        public void ChangeSubTitle(string title) {
            if (subTitle != null) {
                subTitle.SetText(title);
            }
        }

        private void PositionForGrid(int i, GameObject menuItem) {
            menuItem.transform.localPosition = layout.positions[i];
        }

        private static void PositionForRadial(int i, GameObject menuItem) {
            menuItem.transform.localRotation = Quaternion.Euler(0, 0, 45 * i);
        }

        private GameObject GetMenuItemForType(PuzMenuOptionType type) {
            GameObject menuOption = null;
            for (int i = 0; i < types.Count; i++) {
                PuzMenuOptionType optionType = types[i].optionType;
                if (optionType == type || optionType == PuzMenuOptionType.All) {
                    menuOption = Instantiate(types[i].optionPrefab);
                    break;
                }
            }
            return menuOption;
        }

        public void HideMenu() {
            myGameObject.SetActive(false);
            if (navOption != null) {
                navOption.OnHoverEnd -= HandleHoverEnd;
                navOption.OnHoverStart -= HandleHoverStart;
            }
            for (int i = 0; i < itemComps.Count; i++) {
                PuzMenuOptionComp itemComp = itemComps[i];
                itemComp.OnHoverStart -= HandleHoverStart;
                itemComp.OnHoverEnd -= HandleHoverEnd;
            }
            navOption = null;
            subTitle = null;
        }

        public void ShowMenu(Transform menuRoot, PuzMenuOptionComp navigationOption, PuzMenuOptionLabelHandlerComp titleOption, PuzMenuOptionLabelHandlerComp subTitleOption) {
            navOption = navigationOption;
            if (navigationOption != null) {
                navigationOption.OnHoverStart += HandleHoverStart;
                navigationOption.OnHoverEnd += HandleHoverEnd;
            }
            for (int i = 0; i < itemComps.Count; i++) {
                PuzMenuOptionComp itemComp = itemComps[i];
                itemComp.OnHoverStart += HandleHoverStart;
                itemComp.OnHoverEnd += HandleHoverEnd;
            }
            myTransform.position = menuRoot.position;
            myTransform.rotation = menuRoot.rotation;
            myGameObject.SetActive(true);
            titleOption.SetText(menuConfig.screenTitle);
            subTitle = subTitleOption;
            switch (layout.layoutType) {
                case PuzScreenLayoutType.Radial:
                    titleOption.transform.localPosition = new Vector3(0.0f, layout.screenHeight, layout.titlePop);
                    subTitleOption.transform.localPosition = new Vector3(0.0f, layout.screenHeight - layout.titleHeight, layout.titlePop);
                    if (navigationOption != null) {
                        navigationOption.transform.localPosition = Vector3.zero;
                    }
                    break;
                case PuzScreenLayoutType.Grid:
                    titleOption.transform.localPosition = new Vector3(0.0f, layout.screenHeight /2, layout.titlePop);
                    subTitleOption.transform.localPosition = new Vector3(0.0f, (layout.screenHeight / 2) - layout.titleHeight, layout.titlePop);
                    if (navigationOption != null) {
                        navigationOption.transform.localPosition = new Vector3(layout.screenWidth / 2, layout.screenHeight / 2, 0.0f);
                    }
                    break;
            }
        }

        public T FindMenuOption<T>(string id) where T : PuzMenuOptionHandlerComp {
            PuzMenuOptionHandlerComp comp = null;
            for (int i = 0; i < menuOptions.Count(); i++) {
                if (id.Equals(menuOptions[i].OptionId)) {
                    comp = menuOptions[i];
                    break;
                }
            }
            return (T)comp;
        }

        public GameObject GetOptionsRoot() {
            return gameObject;
        }
    }
}
