using System.Collections.Generic;
using System.Linq;
using SSGCore.Custom;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzInput {

    public delegate void PuzButtonEventListener(PuzButtonEventData eventData);

    public class PuzButtonControllerComp : SceneSingletonQuickLoadItem<PuzButtonControllerComp>, CustomUpdater {

        public PuzButtonsConfig config;
        public PuzButtonsState state;
        
        private readonly List<PuzButtonGroupingController> groupingControllers = new List<PuzButtonGroupingController>();
        private readonly Queue<PuzButtonEventData> frameEventQueue = new Queue<PuzButtonEventData>();
        private readonly Dictionary<string, List<ListinerPriority>> listenersMap = new Dictionary<string, List<ListinerPriority>>();

        private PuzButtonGroupingController activeGroupingController;
        private int activeGroupingIndex = -1;

        private class ListinerPriority {
            public int priority;
            public PuzButtonEventListener lisener;
        }

        public bool IsListiningToInput {
            get { return state.isListiningToInput; }
            set { state.isListiningToInput = value; }
        }

        public void AddListener(PuzButtonDriverType driver, PuzButton button, PuzButtonEventListener listener, int priority) {
            string key = GetListenerKey(driver, button);
            List<ListinerPriority> listeners;
            listenersMap.TryGetValue(key, out listeners);
            if (listeners == null) {
                listeners = new List<ListinerPriority>();
                listenersMap[key] = listeners;
            }
            bool isContained = false;
            int insertAfter = 0;
            for (int i = 0; i < listeners.Count; i++) {
                if (listeners[i].lisener == listener) {
                    isContained = true;
                    break;
                }
                if(listeners[i].priority <= priority)
                {
                    insertAfter = i;

                }
            }
            if (!isContained) {
                ListinerPriority item = new ListinerPriority();
                item.lisener = listener;
                item.priority = priority;
                listeners.Insert(insertAfter, item);
            }
        }

        public void RemoveListener(
                PuzButtonDriverType driver, 
                PuzButton button,
                PuzButtonEventListener listener) {
            string key = GetListenerKey(driver, button);
            List<ListinerPriority> listeners;
            listenersMap.TryGetValue(key, out listeners);
            if (listeners != null) {
                int index = -1;
                for (int i = 0; i < listeners.Count; i++) {
                    if (listeners[i].lisener == listener) {
                        
                        index = i;
                        break;
                    }
                }
                if (index >= 0) {
                    listeners.RemoveAt(index);
                }
            }
        }

        public string GetName(PuzButton button, string groupingName) {
            string value = null;
            for (int i = 0; i < config.groupings.Count; i++) {
                PuzButtonGrouping grouping = config.groupings[i];
                if (grouping.name.Equals(groupingName)) {
                    for (int entryIndex = 0; entryIndex < grouping.entries.Count; entryIndex++) {
                        PuzButtonConfigEntry entry = grouping.entries[entryIndex];
                        if (entry.button == button) {
                            value = entry.name;
                            break;
                        }
                    }
                    break;
                }
            }
            return value;
        }

        public string GetActiveIdentifier(PuzButton button) {
            return activeGroupingController.GetActiveIdentifier(button);
        }

        public string GetActiveName(PuzButton button) {
            return activeGroupingController.GetButtonName(button);
        }

        public override void Load() {
            SceneCustomDelegator.AddUpdater(this);
            PopulateGroupingControllers();
            SetDefaultActiveGroupingController();
        }

        private void SetDefaultActiveGroupingController() {
            for (int i = 0; i < groupingControllers.Count; i++) {
                if (groupingControllers[i].GetGroupingType() == config.defaultGrouping) {
                    activeGroupingController = groupingControllers[i];
                    break;
                }
            }
        }

        public override void Unload() {
            
        }

        private void PopulateGroupingControllers() {
            groupingControllers.Clear();
            for (int i = 0; i < config.groupings.Count; i++) {
                PuzButtonGroupingController groupingController = new PuzButtonGroupingController(
                    config.groupings[i], 
                    config.generalConfig,
                    state);
                groupingControllers.Add(groupingController);
                groupingControllers[i].PutGroupToSleep();
            }
        }
        public void DoUpdate() {
            UpdateActiveGroupingController();
            if (activeGroupingController != null) {
                activeGroupingController.EnqueueButtonEvents(frameEventQueue);
                while (frameEventQueue.Any()) {
                    PuzButtonEventData anEvent = frameEventQueue.Dequeue();
                    anEvent.isActualKeyStroke = true;
                    ProcessEvent(anEvent);
                }
            }
        }

        private void UpdateActiveGroupingController() {
            for (int i = 0; i < groupingControllers.Count; i++) {
                if (activeGroupingIndex != i) {
                    string awokenKey = groupingControllers[i].GetAwokenKey();
                    if (awokenKey != null) {
                        bool hasActiveGroupingChange = activeGroupingController != null && !activeGroupingController.ContainsComparisonKey(awokenKey);
                        if (hasActiveGroupingChange || activeGroupingController == null) {
                            if (activeGroupingController != null) {
                                activeGroupingController.PutGroupToSleep();
                            }
                            activeGroupingController = groupingControllers[i];
                            activeGroupingIndex = i;
                            Debug.Log("Active Input Grouping Changed To '" + activeGroupingController.GetName() + "' due to Trigger '" + awokenKey +'\'');
                        }
                        break;
                    }
                }
            }
        }


        private void ProcessEvent(PuzButtonEventData puzButtonEventData) {
            PuzButtonDriverType driver = puzButtonEventData.driverType;
            PuzButton button = puzButtonEventData.button;
            string key = GetListenerKey(driver, button);
            List<ListinerPriority> listeners;
            listenersMap.TryGetValue(key, out listeners);
            if (listeners != null) {
                for (int i = 0; i < listeners.Count; i++) {
                    listeners[i].lisener(puzButtonEventData);
                    if (puzButtonEventData.isConsumed) {
                        break;
                    }
                }
            }
        }

        private static string GetListenerKey(PuzButtonDriverType driver, PuzButton button) {
            return driver + button.ToString();
        }

        public void FireEvent(PuzButtonEventData anEvent) {
            anEvent.isActualKeyStroke = false;
            ProcessEvent(anEvent);
        }

        public List<PuzButtonGrouping> GetGroupings(PuzInputHandedness handedness, bool includeGamepad) {
            List<PuzButtonGrouping> groupings = new List<PuzButtonGrouping>();
            for (int i = 0; i < config.groupings.Count; i++) {
                PuzButtonGrouping grouping = config.groupings[i];
                if (!grouping.isGamePadIncluded || (grouping.isGamePadIncluded && includeGamepad) ) {
                    if (grouping.handedness == handedness) {
                        groupings.Add(grouping);
                    }
                }
            }
            groupings.Sort(new GroupingComparator());
            return groupings;
        }

        private class GroupingComparator : IComparer<PuzButtonGrouping> {
            public int Compare(PuzButtonGrouping x, PuzButtonGrouping y) {
                return x.orderIndex - y.orderIndex;
            }
        }

        public PuzButtonGroupingType GetActiveGroupingType() {
            PuzButtonGroupingType aType = PuzButtonGroupingType.GamePadLeft;
            if (activeGroupingController != null) {
                aType = activeGroupingController.GetGroupingType();
            }
            return aType;
        }

        public PuzInputHandedness GetActiveGroupingHandedness() {
            PuzInputHandedness handedness = PuzInputHandedness.Right;
            if (activeGroupingController != null) {
                return activeGroupingController.Handedness;
            }
            return handedness;
        }
    }
}
