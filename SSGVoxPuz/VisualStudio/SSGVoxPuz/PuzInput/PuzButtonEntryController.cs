using System;
using System.Collections.Generic;
using UnityEngine;


namespace SSGVoxPuz.PuzInput {
    internal class PuzButtonEntryController {

        private readonly PuzButtonConfigEntry config;
        private readonly PuzButtonsGeneralConfig generalConfig;
        private readonly ButtonEntryState state;
        private readonly PuzButtonsState generalState;
        private readonly XInputWrapper xInputWrapper;
        private readonly MouseWrapper mouseWrapper;

        public PuzButtonEntryController(PuzButtonConfigEntry puzButtonConfigEntry, 
                PuzButtonsGeneralConfig aGeneralConfig, 
                PuzButtonsState puzButtonsState) {
            config = puzButtonConfigEntry;
            state = new ButtonEntryState();
            generalState = puzButtonsState;
            generalConfig = aGeneralConfig;
            xInputWrapper = new XInputWrapper(generalConfig.axisTolerance);
            mouseWrapper = new MouseWrapper();
        }

        public bool IsAnythingPressed() {
            return GetButton(config) > 0;
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
                if (duration >= generalConfig.shortToLongPressSeconds && !state.wasLongPressFired) {
                    state.wasLongPressFired = true;
                    EnqueueEventForLongPressConfirm(events);    
                }
                EnqueueButtonPressedContinue(events);
            }
            if (state.isPressedUpThisFrame) {
                float duration = Time.time - state.lastButtonPressDownTime;
                if (duration < generalConfig.shortToLongPressSeconds) {
                    EnqueueEventForShortPressConfirm(events);
                }
                EnqueueButtonPressedUp(events);
            }
        }

        private void UpdateStateForFrame() {
           state.wasButtonPressedLastFrame = state.isButtonPressedThisFrame;
            state.lastFrameValue = state.thisFrameValue;
            state.thisFrameValue = GetButton(config);
            state.isButtonPressedThisFrame = state.thisFrameValue > 0;
           
            
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

        private float GetButton(PuzButtonConfigEntry puzButtonConfigEntry) {
            float value;
            switch (puzButtonConfigEntry.keyType) {
                case KeyType.Keyboard:
                    value = Input.GetKey(config.keyCode) ? 1 : 0;
                    break;
                case KeyType.Mouse:
                    value = mouseWrapper.GetButton(config.mouseButton);
                    break;
                case KeyType.XInput:
                    value = xInputWrapper.GetButton(config.xInputKeyCode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }

        private void EnqueueButtonPressedUp(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button,
                driverType = PuzButtonDriverType.Up,
                eventType = PuzButtonEventType.PressConfirmed,
                value = state.thisFrameValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueButtonPressedContinue(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button, driverType = PuzButtonDriverType.Continue, eventType = PuzButtonEventType.PressConfirmed, value = state.thisFrameValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueButtonPressDown(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button, driverType = PuzButtonDriverType.Down, eventType = PuzButtonEventType.PressConfirmed,
                value = state.thisFrameValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueEventForShortPressConfirm(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button, driverType = PuzButtonDriverType.ShortPress, eventType = PuzButtonEventType.PressConfirmed,
                value = state.thisFrameValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueEventForShortPressStart(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button, driverType = PuzButtonDriverType.ShortPress, eventType = PuzButtonEventType.PressPossibleStart,
                value = state.thisFrameValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueEventForLongPressStart(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button, driverType = PuzButtonDriverType.LongPress, eventType = PuzButtonEventType.PressPossibleStart,
                value = state.thisFrameValue
            };
            events.Enqueue(anEvent);
        }

        private void EnqueueEventForLongPressConfirm(Queue<PuzButtonEventData> events) {
            PuzButtonEventData anEvent = new PuzButtonEventData {
                button = config.button, driverType = PuzButtonDriverType.LongPress, eventType = PuzButtonEventType.PressConfirmed,
                value = state.thisFrameValue
            };
            events.Enqueue(anEvent);
        }

        public void PutEntryToSleep() {
            state.PutToSleep();
        }

        public PuzButton GetPuzButton() {
            return config.button;
        }

        public string GetComparisonKey() {
            return config.GetComparisonKey();
        }

        public string GetName() {
            return config.name;
        }
    }
}
