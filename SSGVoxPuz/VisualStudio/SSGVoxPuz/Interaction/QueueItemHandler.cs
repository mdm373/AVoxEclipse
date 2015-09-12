using UnityEngine;

namespace SSGVoxPuz.Interaction {
    public interface QueueItemHandler {

        void EnqueueItem(Transform item);
        void DequeueItem(Transform item);
    }
}
