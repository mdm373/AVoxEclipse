using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxel.Core;
using SSGVoxPuz.Interaction;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    [Serializable]
    public class PuzToolHandlerBubbleConfig {
        public DragIndicationConfig dragConfig;
        public AudioClip confirmSound;
    }
}
