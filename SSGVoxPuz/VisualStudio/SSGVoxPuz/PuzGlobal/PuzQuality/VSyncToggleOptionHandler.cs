using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzMenu;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzGlobal.PuzQuality {
    class VSyncToggleOptionHandler : PuzToggleMenuOptionHandler {
        private PuzQualityManager qualityManager;

        protected override void HandleOptionSelect(bool isToggled) {
            qualityManager.SetVsyncEnabled(isToggled);
        }

        protected override void HandleOptionInitExtended(GameObject icon, PuzMenuOptionLookup aLookUp) {
            qualityManager = PuzQualityManager.GetSceneLoadInstance();
            IsToggled = qualityManager.IsVsyncEnabled();
            UpdateForToggleState();
        }
    }
}
