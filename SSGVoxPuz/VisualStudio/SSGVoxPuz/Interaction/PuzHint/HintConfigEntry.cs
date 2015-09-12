using System;
using SSGVoxPuz.PuzInput;
using UnityEngine;

namespace SSGVoxPuz.Interaction.PuzHint {
    [Serializable]
    public class HintConfigEntry {

        public SpriteRenderer spritePrefab;
        public PuzButtonGroupingType activeType;
    }
}
