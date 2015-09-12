using System.Collections.ObjectModel;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxel.Textures;
using UnityEngine;

namespace SSGVoxPuz.PuzMod {
    class ModMaterialAlphaHandlerComp : CustomBehaviour {
        
        private Material worldMaterial;
        private VoxelMeshListenerSolidTexture listener;
        private VoxelWorldComp modWorld;

        public void OnEnable() {
            if (CompUtil.IsNull(listener)) {
                modWorld = GetComponentInChildren<VoxelWorldComp>();
                ReadOnlyCollection<VoxelLifeCycleWorldListener> listeners = modWorld.GetListeners();
                for (int i = 0; i < listeners.Count; i++) {
                    VoxelMeshListenerSolidTexture texture = listeners[i] as VoxelMeshListenerSolidTexture;
                    if (texture != null) {
                        VoxelMeshListenerSolidTexture textureListener = texture;
                        listener = textureListener;
                        break;
                    }
                }
            }
        }

        public void SetMaterial(Material material) {
            OnEnable();
            if (worldMaterial == null) {
                worldMaterial = Instantiate(material);
                listener.SetMaterial(worldMaterial);
            }
        }

        public void SetAlpha(float value) {
            Color color = worldMaterial.color;
            color.a = value;
            worldMaterial.color = color;
        }
    }
}
