using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.Persist.Screenshot {
    public class ScreenshotViewer : QuickLoadItem {
        
        
        private ScreenshotController controller;
        public Renderer targetRenderer;
        public Material prefabMaterial;
        
        private Material material;

        public override void Load() {
            controller = ScreenshotController.GetSceneLoadInstance();
            controller.OnScreenshotTaken += HandleScreenshotTaken;

            material = Instantiate(prefabMaterial);
            targetRenderer.material = material;
        }

        private void HandleScreenshotTaken(ScreenshotController arg1, Texture2D arg2) {
            Debug.Log("Viewing Screenshot");
            material.mainTexture = arg2;
        }

        public override void Unload() {
            controller.OnScreenshotTaken -= HandleScreenshotTaken;
        }
    }
}
