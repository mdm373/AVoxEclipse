using System;
using UnityEngine;
using XInputDotNetPure;

namespace SSGVoxPuz.PuzInput
{
    internal class XInputWrapper
    {
        private readonly float deadZone;
        public XInputWrapper(float deadZone) {
            this.deadZone = deadZone;
        }


        public float GetButton(XInputKeyCode code) {
            float value = 0;
            GamePadState state = GamePad.GetState(0);
            if (state.IsConnected) {
                value = GetButton(state, code);
            }
            return value;
        }

        private float GetButton(GamePadState state, XInputKeyCode code) {
            float value;
            switch (code)
            {
                case XInputKeyCode.LeftTrigger:
                    value =  GetTriggerValue(state.Triggers.Left);
                    break;
                case XInputKeyCode.RightTrigger:
                    value = GetTriggerValue(state.Triggers.Right);
                    break;
                case XInputKeyCode.LeftBumper:
                    value = GetButtonValue(state.Buttons.LeftShoulder);
                    break;
                case XInputKeyCode.RightBumper:
                    value = GetButtonValue(state.Buttons.RightShoulder);
                    break;
                case XInputKeyCode.LeftStickClick:
                    value = GetButtonValue(state.Buttons.LeftStick);
                    break;
                case XInputKeyCode.RightStickClick:
                    value = GetButtonValue(state.Buttons.RightStick);
                    break;
                case XInputKeyCode.LeftStickLeft:
                    value = state.ThumbSticks.Left.X < 0 ? GetTriggerValue(state.ThumbSticks.Left.X)  : 0;
                    break;
                case XInputKeyCode.LeftStickRight:
                    value = state.ThumbSticks.Left.X > 0 ? GetTriggerValue(state.ThumbSticks.Left.X)  : 0;
                    break;
                case XInputKeyCode.LeftStickUp:
                    value = state.ThumbSticks.Left.Y > 0 ? GetTriggerValue(state.ThumbSticks.Left.Y)  : 0;
                    break;
                case XInputKeyCode.LeftStickDown:
                    value = state.ThumbSticks.Left.Y < 0 ? GetTriggerValue(state.ThumbSticks.Left.Y)  : 0;
                    break;
                case XInputKeyCode.RightStickUp:
                    value = state.ThumbSticks.Right.Y > 0 ? GetTriggerValue(state.ThumbSticks.Right.Y)  : 0;
                    break;
                case XInputKeyCode.RightStickDown:
                    value = state.ThumbSticks.Right.Y < 0 ? GetTriggerValue(state.ThumbSticks.Right.Y)  : 0;
                    break;
                case XInputKeyCode.RightStickLeft:
                    value = state.ThumbSticks.Right.X < 0 ? GetTriggerValue(state.ThumbSticks.Right.X)  : 0;
                    break;
                case XInputKeyCode.RightStickRight:
                    value = state.ThumbSticks.Right.X > 0 ? GetTriggerValue(state.ThumbSticks.Right.X)  : 0;
                    break;
                case XInputKeyCode.A:
                    value = GetButtonValue(state.Buttons.A);
                    break;
                case XInputKeyCode.B:
                    value = GetButtonValue(state.Buttons.B);
                    break;
                case XInputKeyCode.X:
                    value = GetButtonValue(state.Buttons.X);
                    break;
                case XInputKeyCode.Y:
                    value = GetButtonValue(state.Buttons.Y);
                    break;
                case XInputKeyCode.Start:
                    value = GetButtonValue(state.Buttons.Start);
                    break;
                case XInputKeyCode.Back:
                    value = GetButtonValue(state.Buttons.Back);
                    break;
                case XInputKeyCode.DPadUp:
                    value = GetButtonValue(state.DPad.Up);
                    break;
                case XInputKeyCode.DPadDown:
                    value = GetButtonValue(state.DPad.Down);
                    break;
                case XInputKeyCode.DPadLeft:
                    value = GetButtonValue(state.DPad.Left);
                    break;
                case XInputKeyCode.DPadRight:
                    value = GetButtonValue(state.DPad.Right);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
            return value;
        }

        private float GetButtonValue(ButtonState state) {
            return state == ButtonState.Pressed ? 1 : 0;
        }

        private float GetTriggerValue(float value) {
            return IsDead(value) ? 0 : Mathf.Abs(value);
        }

        private bool IsDead(float value) {
            return Mathf.Abs(value) < deadZone;
        }

        public static bool IsGamePadPresent() {
            return GamePad.GetState(0).IsConnected;
        }
    }
}
