using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SSGVoxPuz.Interaction {

    delegate void TransformQueueEvent(TransformQueue queue, Transform queueHead);

    class TransformQueue {

        public event TransformQueueEvent OnQueueFrontChange;

        private readonly List<Transform> queue = new List<Transform>();
        private Transform oldQueueHead;

        public void Enqueue(Transform item) {
            if (!queue.Contains(item)) {
                queue.Add(item);
                ProcessQueueUpdate();
            }
        }

        public void Dequeue(Transform item) {
            if (queue.Contains(item)) {
                queue.Remove(item);
                ProcessQueueUpdate();   
            }
        }

        private void ProcessQueueUpdate() {
            Transform head = queue.Any() ? queue[0] : null;
            if (head != oldQueueHead) {
                oldQueueHead = head;
                if (OnQueueFrontChange != null) {
                    OnQueueFrontChange(this, oldQueueHead);
                }
            }
        }


    }
}
