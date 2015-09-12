using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Environment {
    class EnvironmentLoader : SceneSingletonSceneLoadItem<EnvironmentLoader> {

        public Environment environmentPrefab;
        private Environment environ;

        public override void Load() {
            GameObject environObject = Instantiate(environmentPrefab.gameObject);
            TransformUtility.ChildAndNormalize(transform, environObject.transform);
            environ = environObject.GetComponent<Environment>();
        }

        public override void Unload() {
            DestroyUtility.DestroyAsNeeded(environ.gameObject);
        }

        public override bool IsLoaded {
            get { return !CompUtil.IsNull(environ) ; }
        }

        public override bool IsUnloaded {
            get { return true; }
        }

        public override void OnNextItemLoading() {
            
        }

        public override void OnNexuItemUnloading() {
            
        }
    }
}
