using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;

namespace SSGVoxPuz.PuzGlobal {
    public abstract class SceneLoadItem : QuickLoadItem {

        public abstract bool IsLoaded { get; }
        public abstract bool IsUnloaded { get; }
        public abstract  void OnNextItemLoading();
        public abstract void OnNexuItemUnloading();


    }
}
