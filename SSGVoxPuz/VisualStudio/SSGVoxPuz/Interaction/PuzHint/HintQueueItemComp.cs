using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSGVoxPuz.Interaction.PuzHint {
    class HintQueueItemComp : QueueItemComp {
        
        public override QueueItemHandler GetHandler() {
            return HintController.GetSceneLoadInstance();
        }
    }
}
