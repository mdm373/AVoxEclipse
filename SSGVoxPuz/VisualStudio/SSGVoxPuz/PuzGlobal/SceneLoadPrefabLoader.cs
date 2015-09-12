using System.Collections;
using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    public class SceneLoadPrefabLoader : SceneLoadItem {

        public SceneLoadItem prefab;
        private PuzController instance;

        public override void Load() {
            GameObject instanceObject = Instantiate(prefab.gameObject);
            instance = instanceObject.GetComponentInChildren<PuzController>();
            TransformUtility.ChildAndNormalize(transform, instanceObject.transform);
            instance.Load();
        }

        public override void Unload() {
            instance.Unload();
            StartCoroutine(UnloadCoroutine());
        }

        private IEnumerator UnloadCoroutine() {
            while (!CompUtil.IsNull(instance)) {
                if (instance.IsUnloaded) {
                    DestroyUtility.DestroyAsNeeded(instance);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public override bool IsLoaded {
            get { return instance.IsLoaded; }
        }

        public override bool IsUnloaded {
            get { return CompUtil.IsNull(instance) || instance.IsUnloaded; }
        }

        public override void OnNextItemLoading() {
            instance.OnNextItemLoading();
        }

        public override void OnNexuItemUnloading() {
            instance.OnNexuItemUnloading();
        }
    }
}
