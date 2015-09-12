using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal {
    public class PuzGlobalLoader : SceneSingletonSceneLoadItem<PuzGlobalLoader> {

        public GameObject globalsPrefab;

        private static PuzGlobalLoader instance;
        private QuickLoadItemList itemList;
        private bool isLoaded;

        public void OnEnable() {
            
        }

        public void OnDestroy() {
            if (instance == this) {
                instance = null;
            }
        }


        public override bool IsLoaded {
            get { return true; }
        }

        public override bool IsUnloaded {
            get { return true; }
        }

        public override void Load() {
            if (!IsThisNotTheLoader() && !isLoaded) {
                isLoaded = true;
                instance = this;
                DontDestroyOnLoad(gameObject);
                GameObject globalsInstance = Instantiate(globalsPrefab);
                globalsInstance.name = "instanced-globals";
                TransformUtility.ChildAndNormalize(transform, globalsInstance.transform);
                itemList = globalsInstance.GetComponent<QuickLoadItemList>();
                itemList.Load();
            }
        }

        public override void Unload() {
            
        }

        private bool IsThisNotTheLoader() {
            return !CompUtil.IsNull(instance) && instance != this;
        }

        public override void OnNextItemLoading() {
            
        }

        public override void OnNexuItemUnloading() {
            
        }
    }
}
