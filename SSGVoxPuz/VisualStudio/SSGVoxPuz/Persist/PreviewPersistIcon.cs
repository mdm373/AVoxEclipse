using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal.PuzFont;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.Persist {
    class PreviewPersistIcon : CustomBehaviour {

        public Material materialPrefab;
        public Renderer targetRenderer;
        public Text incrementText;
        public Text descriptionText;
        public  PuzFontType fontType;
        public Transform toScale;
        
        private Material activeMaterial;
        private Vector2 initialSize = Vector2.zero;

        public void HandleInit() {
            PuzFontController fontController = PuzFontController.GetSceneLoadInstance();
            incrementText.font = fontController.GetFont(fontType);
            descriptionText.font = fontController.GetFont(fontType);
        }

        public void SetScreenshot(Texture2D tex) {
            Material material = GetMaterial();
            material.mainTexture = tex;
            targetRenderer.material = material;
            if (tex != null) {
                UpdateForTextSize(tex);
            }
            else {
                Vector3 size = toScale.localScale;
                size.x = GetInitialSize().x;
                size.y = GetInitialSize().y;
                toScale.localScale = size;
            }
        }

        public void SetIncrementText(string value) {
            incrementText.text = value;
        }

        private void UpdateForTextSize(Texture tex) {
            initialSize = GetInitialSize();
            Vector2 texSize = new Vector2(tex.width, tex.height);
            float texRatio = texSize.x / texSize.y;
            Vector3 scale = toScale.localScale;
            if (texRatio >= 1) {
                scale.x = initialSize.x;
                scale.y = initialSize.y * 1 / texRatio;
            }
            else {
                scale.x = initialSize.x * texRatio;
                scale.y = initialSize.y;
            }
            toScale.localScale = scale;                
        }

        private Vector2 GetInitialSize() {
            if (initialSize == Vector2.zero) {
                initialSize = new Vector2(toScale.localScale.x, toScale.localScale.y);
            }
            return initialSize;
        }

        private Material GetMaterial() {
            if (activeMaterial == null) {
                activeMaterial = Instantiate(materialPrefab);
            }
            return activeMaterial;
        }

        public void SetDescriptionText(string description) {
            descriptionText.text = description;
        }
    }
}
