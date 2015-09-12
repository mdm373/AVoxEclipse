using System;
using System.Globalization;
using System.IO;
using System.Linq;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzMenu;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    public abstract class PersistPreviewOptionHandler : PuzMenuOptionHandlerComp {
        public string incrementFormat = "{0} / {1}";
        private string[] modelFiles;
        protected int previewIndex;
        private PreviewPersistIcon persistIcon;
        protected PersistanceController persistanceController;

        public override void HandleOptionSelect() {
            if (IsValid()) {
                HandleOptionSelected(modelFiles[previewIndex]);
            }
        }

        public bool IsValid() {
            return previewIndex < modelFiles.Count();
        }

        public abstract void HandleOptionSelected(string voxFileName);

        public override void HandleOptionInit(GameObject icon, PuzMenuOptionLookup lookUp) {
            persistanceController = PersistanceController.GetSceneLoadInstance();
            persistanceController.OnSave += UpdateForSave;
            persistanceController.OnDelete += UpdateforDelete;
            modelFiles = PersistanceController.GetAllSaves();
            persistIcon = icon.GetComponent<PreviewPersistIcon>();
            persistIcon.HandleInit();
            UpdateForPreviewIndex();
        }

        private void UpdateforDelete(PersistanceFace obj) {
            HandlePossibleModelsChanged();
        }

        private void UpdateForSave(PersistanceFace obj) {
            HandlePossibleModelsChanged();
        }

        private void HandlePossibleModelsChanged() {
            modelFiles = PersistanceController.GetAllSaves();
            previewIndex = 0;
            UpdateForPreviewIndex();
        }

        public void IncrementPreviewIndex() {
            if (previewIndex < modelFiles.Length - 1) {
                previewIndex++;
            }
            else {
                previewIndex = 0;
            }
            UpdateForPreviewIndex();
            
        }

        public void DecrementSaveIndex() {
            if (previewIndex > 0) {
                previewIndex--;
            }
            else if(modelFiles.Any()) {
                previewIndex = modelFiles.Length - 1;
            }
            UpdateForPreviewIndex();
            
        }

        protected void UpdateForPreviewIndex() {
            long time = 0L;
            Texture2D tex = null;
            if (previewIndex < modelFiles.Count()) {
                string saveFile = modelFiles[previewIndex];
                PersistanceWorldData data = PersistanceController.LoadFile(saveFile);
                PersistanceScreenShot shot = data.screenshot;
                if (shot != null) {
                    tex = new Texture2D(shot.width, shot.height);
                    tex.LoadImage(shot.pngBytes);
                }
                time = data.time;
                
            }
            persistIcon.SetScreenshot(tex);
            UpdateDescriptionText(time);
            UpdateIncrementText();
        }

        private void UpdateDescriptionText(long time) {
            if (time > 0L) {
                DateTime aTime = new DateTime(time);
                string description = aTime.ToString("g", CultureInfo.CreateSpecificCulture("en-us"));
                persistIcon.SetDescriptionText(description);
            }
            else {
                persistIcon.SetDescriptionText("");
            }
        }

        private void UpdateIncrementText() {
            int total = modelFiles.Count();
            int displayIndex = 0;
            if (modelFiles.Any()) {
                displayIndex = previewIndex + 1;
            }
            string incrementText = string.Format(incrementFormat, displayIndex, total);
            persistIcon.SetIncrementText(incrementText);
        }

        public string GetPreviewedModelName() {
            return Path.GetFileNameWithoutExtension(modelFiles[previewIndex]);
        }
    }
}
