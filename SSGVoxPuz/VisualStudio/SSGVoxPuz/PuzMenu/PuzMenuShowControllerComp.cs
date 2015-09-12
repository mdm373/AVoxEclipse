using System.Collections.Generic;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzInput;

namespace SSGVoxPuz.PuzMenu {
    public class PuzMenuShowControllerComp : QuickLoadItem, MenuFaceMenu {

        public bool isPrimaryMenu;
        public PuzButton longPressButton;
        public PuzMenuScreenComp rootMenu;
        private PuzMenuControllerComp _menuControllerComp;

        private Stack<PuzMenuScreenComp> menuStack;
        private PuzController puzController;
        private bool isDisabled;

        public override void Load(){
            PuzButtonController.AddListener(longPressButton, PuzButtonDriverType.LongPress, HandleShowMenu);
            _menuControllerComp = PuzMenuControllerComp.GetSceneLoadInstance();
            menuStack = new Stack<PuzMenuScreenComp>();
            menuStack.Push(rootMenu);
            puzController = PuzController.GetSceneLoadInstance();
            puzController.OnReset += HandleReset;
            if (isPrimaryMenu) {
                puzController.Faces.Menu.Primary = this;
            }
            else {
                puzController.Faces.Menu.Secondary = this;
            }
            _menuControllerComp.OnHide += HandleHidden;
        }

        private void HandleHidden(PuzMenuControllerComp obj) {
            IsOpen = false;
        }

        private void HandleReset(PuzController obj) {
            if (menuStack != null) {
                while (menuStack.Count > 1) {
                    menuStack.Pop();
                }
            }
        }

        public override void Unload() {
            puzController.OnReset -= HandleReset;
            _menuControllerComp.OnHide -= HandleHidden;
        }
        
        private void HandleShowMenu(PuzButtonEventData eventData) {
            if (eventData.eventType == PuzButtonEventType.PressConfirmed) {
                ShowValidatedMenu();
            }
        }

        private void ShowValidatedMenu() {
            if (!_menuControllerComp.IsShown && !isDisabled) {
                _menuControllerComp.ShowMenu(menuStack);
                IsOpen = true;
            }
        }


        public void DisableMenu() {
            isDisabled = true;
        }

        public void EnableMenu() {
            isDisabled = false;
            if (_menuControllerComp.IsShown) {
                _menuControllerComp.HideMenu();
            }
        }

        public bool IsOpen { get; private set; }

        public void ShowMenu() {
            ShowValidatedMenu();
        }
    }
}
