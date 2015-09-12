
using System;
using SSGCore.Custom;

namespace SSGVoxPuz.PuzGlobal {
    
    public abstract class QuickLoadItem : CustomBehaviour{
        public abstract void Load();
        public abstract void Unload();
    }
}
