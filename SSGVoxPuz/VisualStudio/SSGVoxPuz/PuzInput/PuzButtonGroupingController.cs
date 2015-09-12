using System.Collections.Generic;

namespace SSGVoxPuz.PuzInput {
    class PuzButtonGroupingController {

        
        private readonly List<PuzButtonEntryController> entryControllers;
        private readonly PuzButtonGrouping grouping;

        public PuzButtonGroupingController(PuzButtonGrouping aGrouping, PuzButtonsGeneralConfig generalConfig, PuzButtonsState state) {
            entryControllers = new List<PuzButtonEntryController>();
            grouping = aGrouping;
            for (int i = 0; i < aGrouping.entries.Count; i++) {
                entryControllers.Add(new PuzButtonEntryController(aGrouping.entries[i], generalConfig, state));
            }
        }

        public PuzInputHandedness Handedness { get { return grouping.handedness; } }

        public string GetAwokenKey() {
            string awokenKey = null;
            for (int i = 0; i < entryControllers.Count; i++) {
                if (entryControllers[i].IsAnythingPressed()) {
                    awokenKey = entryControllers[i].GetComparisonKey();
                    break;
                }

            }
            return awokenKey;
        }

        public PuzButtonGroupingType GetGroupingType() {
            return grouping.groupingType;
        }

        public void PutGroupToSleep() {
            for (int i = 0; i < entryControllers.Count; i++) {
                entryControllers[i].PutEntryToSleep();
            }
        }

        public void EnqueueButtonEvents(Queue<PuzButtonEventData> events) {
            for (int i = 0; i < entryControllers.Count; i++) {
                entryControllers[i].EnqueueButtonEvents(events);
            }
        }

        public string GetActiveUnityInputName(PuzButton button, out bool isForPositiveOnly) {
            string input = null;
            isForPositiveOnly = false;
            for (int i = 0; i < entryControllers.Count; i++) {
                if (entryControllers[i].GetPuzButton() == button) {
                    input = entryControllers[i].GetUnityInput();
                    isForPositiveOnly = entryControllers[i].IsForPositiveAxisOnly();
                    break;
                }
            }
            return input;
        }

        public bool ContainsComparisonKey(string other) {
            bool isContained = false;
            for (int i = 0; i < entryControllers.Count; i++) {
                if (entryControllers[i].GetComparisonKey().Equals(other)){
                    isContained = true;
                    break;
                }
            }
            return isContained;
        }

        public string GetName() {
            return grouping.name;
        }

    }
}
