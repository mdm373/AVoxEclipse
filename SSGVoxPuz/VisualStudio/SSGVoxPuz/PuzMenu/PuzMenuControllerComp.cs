using System;
using System.Collections.Generic;
using System.Linq;
using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {

    

    public class PuzMenuControllerComp : SceneSingletonQuickLoadItem<PuzMenuControllerComp> {

        public event Action<PuzMenuControllerComp>  OnShow;
        public event Action<PuzMenuControllerComp> OnHide;
        public event Action<PuzMenuControllerComp> OnSelectionEnabled;
        public event Action<PuzMenuControllerComp> OnSelectionDisabled;

        public AudioClip openSound;
        public PuzMenuOptionConfig closeOptionConfig;
        public PuzMenuOptionConfig titleOptionConfig;
        public bool isOpenSilent;

        public GameObject closeItemPrefab;
        public GameObject closeIconPrefab;
        public GameObject backIconPrefab;
        public GameObject titleItemPrefab;
        public GameObject titleIconPrefab;
        public GameObject subTitleItemPrefab;
        public GameObject subTitleIconPrefab;
        public GameObject emptyIconPrefab;
        public PuzMenuSelectionConfig selectionConfig;
        public Vector2 hudOffset = Vector2.zero;

        private Stack<PuzMenuScreenComp> stackedMenus;
        private PuzMenuOptionLabelHandlerComp titleHandler;
        private PuzMenuOptionCloseHandlerComp closeHandler;
        private PuzMenuOptionLabelHandlerComp subTitleHandler;
        private PuzMenuOptionComp navigationItem;
        private PuzMenuOptionComp titleItem;
        private PuzMenuOptionComp subTitleItem;
        private PuzMenuScreenComp rootMenu;
        private bool isEnabled;
        public bool isClosable = true;

        public bool IsShown { get; private set; }
        public bool IsMenuInteractionEnabled { get; private set; }
        public Transform CameraRoot { get; private set; }

        protected override void HandleCacheRequest() {
            CameraRoot = PuzMenuCameraRootComp.GetSceneInstance().transform;
        }

        public override void Load() {
            isEnabled = true;
            AllocatDecoraterItems();
            IsShown = false;
            InitDecoratorItems();
            if (OnHide != null) {
                OnHide(this);
            }
            HudController.GetSceneInstance().ShowHudItem(gameObject, hudOffset);
        }

        public override void Unload() {
            HudController.GetSceneInstance().RemoveHudItem(gameObject);
        }

        private void InitDecoratorItems() {
            InitDecoratorItem (ref titleHandler, "title", titleItem, titleIconPrefab, titleOptionConfig);
            InitDecoratorItem(ref subTitleHandler, "sub-title", subTitleItem, subTitleIconPrefab, titleOptionConfig);
            if (isClosable) {
                InitDecoratorItem(ref closeHandler, "close", navigationItem, closeIconPrefab, closeOptionConfig);
            }
            else {
                InitDecoratorItem(ref closeHandler, string.Empty, navigationItem, emptyIconPrefab, closeOptionConfig);
            }
        }

        private void AllocatDecoraterItems() {
            InstantiateOption(ref navigationItem, closeItemPrefab, "nav-item");
            InstantiateOption(ref titleItem, titleItemPrefab, "title-item");
            InstantiateOption(ref subTitleItem, subTitleItemPrefab, "sub-title-item");
        }

        // ReSharper disable once RedundantAssignment (Reassigned field reference
        private static void InitDecoratorItem<T>(ref T handler, string hoverText,PuzMenuOptionComp item,GameObject iconPrefab, PuzMenuOptionConfig config)  where T : PuzMenuOptionHandlerComp{
            handler = item.gameObject.AddComponent<T>();
            handler.IconPrefab = iconPrefab;
            handler.HoverText = hoverText;
            item.Init(handler, config, null);
            item.gameObject.SetActive(false);
        }

        private void InstantiateOption(ref PuzMenuOptionComp item, GameObject prefab, string itemName) {
            if (!CompUtil.IsNull(item)) {
                DestroyUtility.DestroyAsNeeded(item);
            }
            GameObject itemObject = Instantiate(prefab);
            itemObject.name = itemName;
            TransformUtility.ChildAndNormalize(gameObject.transform, itemObject.transform);
            item = itemObject.GetComponent<PuzMenuOptionComp>();
        }

        public void ShowMenu(Stack<PuzMenuScreenComp> menuStack) {
            if (!IsShown && isEnabled) {
                if (!isOpenSilent) {
                    AudioSource.PlayClipAtPoint(openSound, transform.position);
                }
                stackedMenus = menuStack;
                PuzMenuScreenComp aMenu = menuStack.Pop();
                IsShown = true;
                rootMenu = aMenu;
                if (OnSelectionEnabled != null) {
                    OnSelectionEnabled(this);
                }
                rootMenu.ShowMenu(transform, navigationItem, titleHandler, subTitleHandler);
                navigationItem.gameObject.SetActive(true);
                if (isClosable || menuStack.Any()) {
                    closeHandler.IconPrefab = menuStack.Any() ? backIconPrefab : closeIconPrefab;
                }
                navigationItem.UpdateIcon();
                titleItem.gameObject.SetActive(true);
                subTitleItem.gameObject.SetActive(true);
                IsMenuInteractionEnabled = true;

                
                if (OnShow != null) {
                    OnShow(this);
                }
            }
        }

        public void NavToMenu(PuzMenuScreenComp toNavTo) {
            if (IsShown) {
                closeHandler.HoverText = "back";
                rootMenu.HideMenu();
                stackedMenus.Push(rootMenu);
                rootMenu = toNavTo;
                rootMenu.ShowMenu(transform, navigationItem, titleHandler, subTitleHandler);
                
                closeHandler.IconPrefab = backIconPrefab;
                navigationItem.UpdateIcon();
            }
        }

        public void NavBack() {
            if (stackedMenus.Any()) {
                rootMenu.HideMenu();
                rootMenu = stackedMenus.Pop();
                if (!stackedMenus.Any()) {
                    closeHandler.HoverText = (isClosable) ? "close" : String.Empty;
                    closeHandler.IconPrefab = isClosable ? closeIconPrefab : emptyIconPrefab;
                    navigationItem.UpdateIcon();
                    rootMenu.ChangeSubTitle(closeHandler.HoverText);                    
                }
                rootMenu.ShowMenu(transform, navigationItem, titleHandler, subTitleHandler);
            }
        }
        

        public void HideMenu() {
            if (IsShown && isClosable) {
                IsShown = false;
                if (OnSelectionDisabled != null) {
                    OnSelectionDisabled(this);
                }
                navigationItem.gameObject.SetActive(false);
                titleItem.gameObject.SetActive(false);
                subTitleItem.gameObject.SetActive(false);
                rootMenu.HideMenu();
                IsMenuInteractionEnabled = false;
                stackedMenus.Push(rootMenu);
                if (OnHide != null) {
                    OnHide(this);
                }
            }
        }

        public bool IsBackAvailable() {
            return stackedMenus.Any();
        }

        public PuzMenuSelectionConfig SelectionConfig {
            get { return selectionConfig; }
        }

        public MenuFaceMenu Primary { get; set; }
        public MenuFaceMenu Secondary { get; set; }

        public void SetClosable(bool aIsClosable) {
            isClosable = aIsClosable;
        }
    }
}
