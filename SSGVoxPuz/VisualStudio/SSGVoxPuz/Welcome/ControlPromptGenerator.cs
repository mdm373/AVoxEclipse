using System.Collections.Generic;
using System.Linq;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzInput;
using SSGVoxPuz.PuzInput.PuzInputUI;
using SSGVoxPuz.PuzTutorial.TutorialPrompt;
using SSGVoxPuz.Utility;
using UnityEngine;

namespace SSGVoxPuz.Welcome {
    public class ControlPromptGenerator : QuickLoadItem {

        public PuzInputHandedness handedness;
        public PuzButton confirmButton = PuzButton.Primary;
        public float offset;
        public TutorialButtonPrompt promptPrefab;
        public GameObject paddingPrefab;

        public override void Load() {
            bool includeGamepad = IsGamepadIncluded();
            List<PuzButtonGrouping> groupings = PuzButtonController.GetGroupings(handedness, includeGamepad);
            if (groupings.Any()) {


                float leftStart = 0;
                for (int i = 0; i < groupings.Count; i++) {
                    Vector3 position = new Vector3(leftStart, 0.0f, 0.0f);
                    GameObject backingObj = Instantiate(promptPrefab.gameObject);
                    TransformUtility.ChildAndNormalize(transform, backingObj.transform);
                    backingObj.transform.localPosition = position;
                    TutorialButtonPrompt prompt = backingObj.GetComponent<TutorialButtonPrompt>();
                    
                    InputUiIcon icon = InputUiController.GetSceneLoadInstance().GetIcon(groupings[i].name, confirmButton);
                    LayerUtil.RelayerAll(icon.gameObject, prompt.gameObject.layer);
                    TransformUtility.ChildAndNormalize(prompt.iconAnchor, icon.transform);
                    leftStart += offset;
                    if (i < groupings.Count - 1) {
                        Vector3 paddingPosition = new Vector3(leftStart, 0.0f, 0.0f);
                        GameObject padding = Instantiate(paddingPrefab);
                        LayerUtil.RelayerAll(padding.gameObject, prompt.gameObject.layer);
                        TransformUtility.ChildAndNormalize(transform, padding.transform);
                        padding.transform.localPosition = paddingPosition;
                        leftStart += offset;
                    }
                }                
            }
        }

        private static bool IsGamepadIncluded() {
            return XInputWrapper.IsGamePadPresent();
        }

        public override void Unload() {
            
        }
    }
}
