using System;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface PersistanceFace {
        void LoadWorld(TextAsset asset);
        void SaveWorld(string saveName);
        event Action<PersistanceFace> OnSave;
    }
}
