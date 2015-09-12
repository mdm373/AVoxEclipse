using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.PuzQuality {
    class PuzQualityLevelMenuOptionHandler : PuzMenuOptionHandlerComp {
        public bool isIncrease;
        
        private PuzQualityManager qualityController;

        public override void HandleOptionSelect() {
            if (isIncrease) {
                qualityController.IncreaseQuality();
            }
            else {
                 qualityController.DecreaseQuality();
            }
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            qualityController = PuzQualityManager.GetSceneLoadInstance();
        }
    }
}
