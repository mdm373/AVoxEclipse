namespace SSGVoxPuz.PuzInput {
    class ButtonEntryState {
        public float lastFrameValue;
        public float thisFrameValue;
        public bool isButtonPressedThisFrame;
        public bool wasButtonPressedLastFrame;
        public float lastButtonPressDownTime;
        public float lastButtonPressUpTime;
        public bool isPressedUpThisFrame;
        public bool isPressedDownThisFrame;
        public bool wasLongPressFired;

        public void PutToSleep() {
            isButtonPressedThisFrame = false;
            wasButtonPressedLastFrame = false;
            lastFrameValue = 0.0f;
            thisFrameValue = 0.0f;
            lastButtonPressUpTime = float.MinValue;
            lastButtonPressDownTime = float.MinValue;
            isPressedUpThisFrame = false;
            isPressedDownThisFrame = false;
            wasLongPressFired = false;
        }
    }
}
