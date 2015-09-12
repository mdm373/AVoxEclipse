using System;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.PuzQuality {
    class PuzQualityManager : SceneSingletonQuickLoadItem<PuzQualityManager> {
        
        public Action<PuzQualityManager> onQualityLevelChange;
        public Action<PuzQualityManager> onVsyncChanged;

        public override void Load() {
        
        }

        public override void Unload() {
            
        }

        public void IncreaseQuality() {
            bool wasVSyncEnabled = QualitySettings.vSyncCount > 0;
            QualitySettings.IncreaseLevel(true);
            FireQualityChanged();
            QualitySettings.vSyncCount = (wasVSyncEnabled) ? 1 : 0;
        }

        public void DecreaseQuality() {
            bool wasVSyncEnabled = QualitySettings.vSyncCount > 0;
            QualitySettings.DecreaseLevel(true);
            FireQualityChanged();
            QualitySettings.vSyncCount = (wasVSyncEnabled) ? 1 : 0;
        }

        private void FireQualityChanged() {
            if (onQualityLevelChange != null) {
                onQualityLevelChange(this);
            }
        }

        public string GetCurrentQualityLevelDescription() {
            return QualitySettings.names[QualitySettings.GetQualityLevel()];
        }

        public bool IsVsyncEnabled() {
            return QualitySettings.vSyncCount > 0;
        }

        public void SetVsyncEnabled(bool isToggled) {
            QualitySettings.vSyncCount = (isToggled) ? 1 : 0;
            FireVSyncChanged();
        }

        private void FireVSyncChanged() {
            if (onVsyncChanged != null) {
                onVsyncChanged(this);
            }
        }
    }
}
