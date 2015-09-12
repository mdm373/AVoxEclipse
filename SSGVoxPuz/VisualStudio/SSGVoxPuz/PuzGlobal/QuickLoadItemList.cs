using System.Collections.Generic;

namespace SSGVoxPuz.PuzGlobal {
    public class QuickLoadItemList : SceneLoadItem {

        public List<QuickLoadItem> items;

        public override void Load() {
            for (int i = 0; i < items.Count; i++) {
                items[i].Load();
            }
        }

        public override void Unload() {
            for (int i = items.Count - 1; i >= 0; i--) {
                items[i].Unload();
            }
        }

        public override bool IsLoaded {
            get { return true; }
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
