using SSGCore.Custom;
using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    public abstract class PuzDynaMenuBuilder : CustomBehaviour {

        public abstract void BuildDynamicItems(GameObject options, PuzScreenLayoutConfig layout);
    }
}
