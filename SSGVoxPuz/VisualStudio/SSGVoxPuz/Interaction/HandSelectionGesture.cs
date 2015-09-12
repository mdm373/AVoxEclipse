using SSGCore.Custom;
using SSGCore.Utility;
using UnityEngine;

namespace SSGVoxPuz.Interaction {

    public class HandSelectionGesture : QueueItemComp {
        
        
        public override QueueItemHandler GetHandler() {
            return SelectionControllerComp.GetSceneLoadInstance();
        }
    }
}
