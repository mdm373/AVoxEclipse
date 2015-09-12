using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSGVoxPuz.PuzInput {
    class PuzButtonEntryController {

        private readonly PuzButtonConfigEntry config;
        private readonly PuzButtonsGeneralConfig generalConfig;
        private readonly ButtonEntryState state;
        private readonly PuzButtonsState generalState;

        public PuzButtonEntryController(PuzButtonConfigEntry puzButtonConfigEntry, 
                PuzButtonsGeneralConfig aGeneralConfig, 
                PuzButtonsState puzButtonsState) {
            config = puzButtonConfigEntry;
            state = new ButtonEntryState();
            generalState = puzButtonsState;
            generalConfig = aGeneralConfig;
        }

        public bool IsAnythingPressed() {
            return IsButtonPressedAtAll();
        }

        public void EnqueueButtonEvents(Queue<PuzButtonEventData> events) {
            if (generalState.isListiningToInput || config.isAlwaysListenedTo) {
                UpdateStateForFrame();
                EnqueueAllEvents(events);   
            }
        }

        private void EnqueueAllEvents(Queue<PuzButtonEventData> events) {
            if (state.isPressedDownThisFrame) {
                EnqueueEventForLongPressStart(events);
                EnqueueEventForShortPressStart(events);
                EnqueueButtonPressDown(events);
            }
            if (state.isButtonPressedThisFrame) {
                float duration = Time.time - state.lastButtonPressDownTime;
                if (duration >= generalConfig.shortToLongPressSeconds && !state.wasLongPressFired && !config.isAxis) {
                    state.wasLongPressFired = true;
                    EnqueueEventForLongPressConfirm(events);    
                }
                EnqueueButtonPressedContinue(events);
            }
            if (state.isPressedUpThisFrame) {
                float duration = Time.time - state.lastButtonPressDownTime;
                if (duration < generalConfig.shortToLongPressSeconds && !config.isAxis) {
                    EnqueueEventForShortPressConfirm(events);
                }
                EnqueueButtonPressedUp(events);
            }
        }

        private void UpdateStateForFrame() {
            if (config.isUnityInputAxis) {
                float axisMod = config.isAxisInverted ? -1f : 1f;
                state.lastFrameUnityAxisValue = state.thisFrameUnityAxisValue;
                state.thisFrameUnityAxisValue = axisMod * Input.GetAxis(config.unityInputName);

                if (config.unityInputName.Equals("Xbox-Triggers")) {
                    //Debug.Log(state.thisFrameUnityAxisValue);
                }

                state.wasButtonPressedLastFrame = state.isButtonPressedThisFrame;
                if (config.isAxis) {
                    state.isButtonPressedThisFrame = IsUnityAxisActive(state.thisFrameUnityAxisValue);
                }
                else { //Is Button
                    state.isButtonPressedThisFrame = IsAxisPressedForValue(state.thisFrameUnityAxisValue);
                }
            }
            else {
                state.wasButtonPressedLastFrame = state.isButtonPressedThisFrame;
                state.isButtonPressedThisFrame = Input.GetButton(config.unityInputName);
            }
            
            if (!state.wasButtonPressedLastFrame && state.isButtonPressedThisFrame) {
                state.lastButtonPressDownTime = Time.time;
                state.isPressedDownThisFrame = true;
            }
            else {
                state.isPressedDownThisFrame = false;
            }

            if (state.wasButtonPressedLastFrame && !state.isButtonPressedThisFrame) {
                state.lastButtonPressUpTime = Time.time;
                state.isPressedUpThisFrame = true;
                state.wasLongPressFired = false;
            }
            else {
                state.isPressedUpThisFrame = false;
            }

        }


        private bool IsButtonPressedAtAll() {
            bool isPressedAtAll;
            if (config.isUnityInputAxis) {
                float axisValue = Input.GetAxis(config.unityInputName);
                isPressedAtAll = config.isAxis ? IsUnityAxisActive(axisValue) : IsAxisPressedForValue(axisValue);
            }
            else {
                isPressedAtAll = Input.GetButton(config.unityInputName);
            }
            return isPressedAtAll;
        }

        private bool IsUnityAxisActive(float axisValue) {
            return Mathf.Abs(axisValue) >= generalConfig.axisTolerance;
        }

        private bool IsAxisPressedForValue(float axisValue) {
            bool isIdle = Mathf.Abs(axisValue) < generalConfig.axisTolerance;
            return !isIdle && (config.isForPositiveUnityAxis) ? axisValue > 0 : axisValue < 0;
        }

        private void EnqueueButtonPressedUp(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.Up,
                eventType = PuzButtonEventType.PressConfirmed,
                axisDirection = state.thisFrameUnityAxisValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueButtonPressedContinue(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.Continue,
                eventType = PuzButtonEventType.PressConfirmed,
                axisDirection = state.thisFrameUnityAxisValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueButtonPressDown(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.Down,
                eventType = PuzButtonEventType.PressConfirmed,
                axisDirection = state.thisFrameUnityAxisValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueEventForShortPressConfirm(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.ShortPress,
                eventType = PuzButtonEventType.PressConfirmed,
                axisDirection = state.thisFrameUnityAxisValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueEventForShortPressStart(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.ShortPress,
                eventType = PuzButtonEventType.PressPossibleStart,
                axisDirection = state.thisFrameUnityAxisValue
            };
            events.Enqueue(anEvent);
        }
        
        private void EnqueueEventForLongPressStart(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.LongPress,
                eventType = PuzButtonEventType.PressPossibleStart,
                axisDirection = state.thisFrameUnityAxisValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueEventForLongPressConfirm(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.LongPress,
                eventType = PuzButtonEventType.PressConfirmed,
                axisDirection = state.thisFrameUnityAxisValue
            };
            events.Enqueue(anEvent);
        }

        public void PutEntryToSleep() {
            state.PutToSleep();
        }

        public PuzButton GetPuzButton() {
            return config.button;
        }

        public string GetUnityInput() {
            return config.unityInputName;
        }

        public bool IsForPositiveAxisOnly() {
            return config.isForPositiveUnityAxis;
        }

        public string GetComparisonKey() {
            return GetUnityInput() + IsForPositiveAxisOnly();
        }
    }
}
