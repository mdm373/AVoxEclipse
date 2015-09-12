using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using UnityEngine;

namespace SSGVoxPuz.PuzTutorial.PuzTriggerEvent {
    public class WaitForZoom : BaseWaitEvent {

        public int requiredZoomCount = 5;
        
        private float oldZoomLevel ;
        private int currentZoomCount;
        private int currentSign;
        private ZoomFace zoom;
        

        protected override float GetCurrentValue() {
            return currentZoomCount;
        }

        protected override float GetRequiredValue() {
            return requiredZoomCount;
        }

        protected override void UpdateTrackedValues() {
            float zoomLevel = zoom.GetZoomLevel();
            int nowSign = GetSign(zoomLevel);
            if (nowSign != currentSign) {
                currentZoomCount++;
            }
            currentSign = nowSign;
            oldZoomLevel = zoomLevel;
        }

        private int GetSign(float zoomLevel) {
            int sign = 0;
            float nowChange = zoomLevel - oldZoomLevel;
            if (Mathf.Abs(nowChange) > .001f) {
                sign = -1;
                if (nowChange > 0) {
                    sign = 1;
                }
            }
            return sign;
        }

        protected override void HandleExtendedStarted() {
            zoom = PuzController.GetSceneLoadInstance().Faces.Zoom;
            zoom.HaultActiveZooming();
            oldZoomLevel = zoom.GetZoomLevel();
            currentZoomCount = 0;
            currentSign = 0;
        }

        protected override bool ShouldShake() {
            return GetSign(zoom.GetZoomLevel()) != currentSign;
        }

        protected override bool IsNowFinished() {
            return currentZoomCount >= requiredZoomCount;
        }

        protected override void HandleExtendedExit() {
            
        }
    }
}
