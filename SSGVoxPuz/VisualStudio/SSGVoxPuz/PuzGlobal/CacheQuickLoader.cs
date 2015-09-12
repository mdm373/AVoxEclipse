using SSGCore.Custom;

namespace SSGVoxPuz.PuzGlobal {
    class CacheQuickLoader : CustomBehaviour {
        
        
        
        protected override void HandleCacheRequest() {
            GetComponent<QuickLoadItem>().Load();
        }
    }
}
