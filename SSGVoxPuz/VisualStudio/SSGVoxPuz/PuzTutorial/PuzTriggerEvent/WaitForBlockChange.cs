using System.Collections.Generic;
using SSGVoxel.APIS;
using SSGVoxPuz.PuzGlobal;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    public class WaitForBlockChange : BaseWaitEvent{
        public bool requireAllNewTypes;
        public bool requireTypeChanged = true;
        public float requiredModCount = 10;
        private int modCount;
        private int trackedModCount;
        private bool isShouldShake;
        private readonly List<VoxelBlockType> modTypes = new List<VoxelBlockType>(); 

        protected override void HandleExtendedStarted() {
            trackedModCount = 0;
            modCount = 0;
            modTypes.Clear();
            PuzController.GetSceneLoadInstance().Faces.Modification.OnModification += HandleModification;
        }

        private void HandleModification(VoxelWorldPosition position, VoxelBlockType oldValue, VoxelBlockType newValue) {
            if ((requireAllNewTypes && !modTypes.Contains(newValue)) || !requireAllNewTypes) {
                if (requireTypeChanged && oldValue.ByteCode != newValue.ByteCode || !requireTypeChanged) {
                    modTypes.Add(newValue);
                    modCount++;
                    if (modCount < requiredModCount) {
                        isShouldShake = true;
                    }    
                }
            }
        }

        protected override float GetCurrentValue() {
            return trackedModCount;
        }

        protected override float GetRequiredValue() {
            return requiredModCount;
        }

        protected override void UpdateTrackedValues() {
            trackedModCount = modCount;
        }

        protected override void HandleExtendedExit() {
            
        }

        protected override bool ShouldShake() {
            bool wasShouldShake = isShouldShake;
            isShouldShake = false;
            return wasShouldShake;
        }

        protected override bool IsNowFinished() {
            return trackedModCount >= requiredModCount;
        }
    }
}
