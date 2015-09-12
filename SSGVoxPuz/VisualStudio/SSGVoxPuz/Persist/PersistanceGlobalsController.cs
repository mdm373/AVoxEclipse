using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SSGVoxPuz.Persist {
    public class PersistanceGlobalsController : SceneSingletonBehaviour<PersistanceGlobalsController> {

        public string winLocation = @"%AppData%\AirspaceApps\[AppName]";
        public string macLocation = @"$HOME/Library/Application Support/AirspaceApps/[AppName]";
        public bool useDefaultUnityLocation;

        public List<RuntimePlatform> macPlats;
        
        public string GetSaveLocation() {
            string location = winLocation;
            if (useDefaultUnityLocation) {
                location = Application.persistentDataPath;
            }
            else {
                RuntimePlatform platform = Application.platform;
                if (macPlats.Contains(platform)) {
                    location = macLocation;
                }
                else {
                    string prefix = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                    location = prefix + winLocation;
                }
            }
            return location;
        }
    }
}
