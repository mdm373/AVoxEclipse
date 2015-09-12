using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.SceneNavigation {
    public class SceneTransitionMenuItem : PuzMenuOptionHandlerComp {
        
        private SceneNavigationController navigator;
        public PuzSceneType targetScene;

        public override void HandleOptionSelect() {
            navigator.RequestSceneTransition(targetScene);    
        }

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            navigator = SceneNavigationController.GetSceneLoadInstance();
        }
    }
}
