using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {

    public interface MenuFaceMenu {
        void DisableMenu();
        void EnableMenu();
        bool IsOpen { get; }
        void ShowMenu();
    }
    
    public interface MenuFace {
        MenuFaceMenu Primary { get; set; }
        MenuFaceMenu Secondary { get; set; }
        void HideMenus();
    }
}
