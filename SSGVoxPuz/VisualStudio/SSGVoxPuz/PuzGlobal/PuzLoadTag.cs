using SSGCore.Custom;
using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    public class PuzLoadTag : CustomBehaviour {
        public GameObject toLoad;
        public bool isLoadedOnEnable;
        
        private bool isloaded;
        private GameObject loaded;

        public GameObject LoadedObject {
            get { return loaded; }
        }

        public bool IsLoaded {
            get { return isloaded;}
        }


        public void OnEnable() {
            if (isLoadedOnEnable && !isloaded) {
                HandleLoadRequest();
            }
        }

        public void Load() {
            if (!isloaded) {
                HandleLoadRequest();
            }
        }

        private void HandleLoadRequest() {
            isloaded = true;
            loaded = Instantiate(toLoad);
            TransformUtility.ChildAndNormalize(transform, loaded.transform);
            HandleExtendedLoad(loaded);
        }

        public virtual void HandleExtendedLoad(GameObject loadedObject) {
            
        }
    }
}
