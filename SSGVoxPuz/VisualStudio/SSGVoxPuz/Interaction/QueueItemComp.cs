using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGCore.Custom;
using SSGCore.Utility;

namespace SSGVoxPuz.Interaction {
    public abstract class QueueItemComp : CustomBehaviour {
        
        private QueueItemHandler itemHandler;

        public abstract QueueItemHandler GetHandler();

        public void OnEnable() {
            itemHandler = GetHandler();
            if (!CompUtil.IsNull(itemHandler)) {
                itemHandler.EnqueueItem(transform);
            }
        }


        public void OnDestroy() {
            if (!CompUtil.IsNull(itemHandler)) {
                itemHandler.DequeueItem(transform);
            }
        }

    }
}
