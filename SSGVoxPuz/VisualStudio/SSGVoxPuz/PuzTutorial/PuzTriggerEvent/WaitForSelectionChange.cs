using System.Collections;
using System.Collections.Generic;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzTutorial.TutorialEvent;
using SSGVoxPuz.PuzTutorial.TutorialText;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    class WaitForSelectionChange : BaseWaitEvent {

        public int requiredCount;
        private int currentCount;
        private bool shouldShake;
        private SelectionFace selection;

        private IEnumerator<YieldInstruction> DelayStart() {
            shouldShake = false;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            selection = PuzController.GetSceneLoadInstance().Faces.Selection;
            selection.OnSelectionChange += HandleSelectionChange;
        }
        
        protected override float GetCurrentValue() {
            return currentCount;
        }

        protected override float GetRequiredValue() {
            return requiredCount;
        }

        protected override void UpdateTrackedValues() {
            
        }

        protected override void HandleExtendedStarted() {
            StartCoroutine(DelayStart());
        }

        protected override bool ShouldShake() {
            bool wasShouldShake = shouldShake;
            shouldShake = false;
            return wasShouldShake;
        }

        protected override bool IsNowFinished() {
            return currentCount >= requiredCount;
        }

        protected override void HandleExtendedExit() {
            if (selection != null) {
                selection.OnSelectionChange -= HandleSelectionChange;
            }
        }

        private void HandleSelectionChange(SelectionFace obj) {
            currentCount++;
            if (currentCount != requiredCount) {
                shouldShake = true;
            }
        }

    }
}
