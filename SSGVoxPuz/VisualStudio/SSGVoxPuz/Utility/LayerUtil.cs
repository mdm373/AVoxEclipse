using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SSGVoxPuz.Utility {
    class LayerUtil {

        public static void RelayerAll(GameObject aGameObject, int layer) {
            for (int i = 0; i < aGameObject.transform.childCount; i++) {
                Transform child = aGameObject.transform.GetChild(i);
                child.gameObject.layer = layer;
                RelayerAll(child.gameObject, layer);

            }
        }
    }
}
