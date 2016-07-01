using System;
using UnityEngine;

namespace SSGVoxPuz.PuzInput
{
    internal class MouseWrapper
    {
        public float GetButton(MouseButton mouseButton) {
            float value = 0;
            if (Input.mousePresent) {
                value = GetPresentButton(mouseButton);
            }
            return value;
        }

        private float GetPresentButton(MouseButton mouseButton) {
            float value;
            switch (mouseButton) {
                case MouseButton.Left:
                    value =  Input.GetMouseButton(0) ? 1 : 0;
                    break;
                case MouseButton.ScrollUp:
                    value = Input.mouseScrollDelta.y > 0 ? Mathf.Abs(Input.mouseScrollDelta.y) : 0;
                    break;
                case MouseButton.ScrollDown:
                    value = Input.mouseScrollDelta.y < 0 ? Mathf.Abs(Input.mouseScrollDelta.y) : 0;
                    break;
                case MouseButton.ScrollClick:
                    value = Input.GetMouseButton(2) ? 1 : 0;
                    break;
                case MouseButton.Right:
                    value = Input.GetMouseButton(1) ? 1 :0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton, null);
            }
            return value;
        }
    }
}
