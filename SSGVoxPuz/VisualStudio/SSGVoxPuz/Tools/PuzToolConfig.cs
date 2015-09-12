using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    [Serializable]
    public struct PuzToolConfig {
        public GameObject iconPrefab;
        public GameObject hoverPrefab;
        public PuzTool tool;
        public string description;
        public AudioClip requestSound;
    }
}
