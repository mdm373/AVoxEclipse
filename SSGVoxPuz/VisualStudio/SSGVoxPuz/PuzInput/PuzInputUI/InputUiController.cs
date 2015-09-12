using System;
using System.Collections.Generic;
using SSGCore.Utility;
using SSGVoxPuz.PuzGlobal;
using UnityEngine;

namespace SSGVoxPuz.PuzInput.PuzInputUI {
    public class InputUiController :SceneSingletonQuickLoadItem<InputUiController> {

        public List<InputUiIcon> iconPrefabs;
        public List<InputUiButtonConfig> configs;
        public List<GroupingConfigEntry> groupingConfigs;
        private Dictionary<InputUiButtonType, InputUiIcon> iconPrefabMap;
        private Dictionary<string, InputUiButtonConfig> configMap;
        private Dictionary<PuzButtonGroupingType, GroupingConfigEntry> groupingConfigMap;
        
        public override void Load() {
            iconPrefabMap = new Dictionary<InputUiButtonType, InputUiIcon>();
            for (int i = 0; i < iconPrefabs.Count; i++) {
                InputUiButtonType type = iconPrefabs[i].buttonType;
                iconPrefabMap[type] = iconPrefabs[i];
            }
            configMap = new Dictionary<string, InputUiButtonConfig>();
            for (int i = 0; i < configs.Count; i++) {
                string unityInputName = configs[i].name;
                configMap[GetConfigKey(unityInputName, configs[i].isPositiveOnly)] = configs[i];
            }
            groupingConfigMap = new Dictionary<PuzButtonGroupingType, GroupingConfigEntry>();
            for (int i = 0; i < groupingConfigs.Count; i++) {
                groupingConfigMap[groupingConfigs[i].grouping] = groupingConfigs[i];
            }

        }

        public override void Unload() {
            
        }
        
        public InputUiIcon GetIcon(PuzButton button) {
            bool isForPositiveOnly;
            string unityName = PuzButtonController.GetActiveUnityInputName(button, out isForPositiveOnly);
            return GetIconForUnityName(unityName, isForPositiveOnly);
        }

        private InputUiIcon GetIconForUnityName(string unityName, bool isForPositiveOnly) {
            InputUiButtonConfig config = configMap[GetConfigKey(unityName, isForPositiveOnly)];
            InputUiIcon prefab = iconPrefabMap[config.type];
            GameObject iconObject = Instantiate(prefab.gameObject);
            InputUiIcon icon = iconObject.GetComponent<InputUiIcon>();
            icon.text.text = config.displayText;
            icon.IsAxis = config.isAxis;
            return icon;
        }

        public InputUiIcon GetIcon(string groupingName, PuzButton button) {
            bool isForPositiveOnly;
            string unityName = PuzButtonController.GetUnityInputName(button, groupingName, out isForPositiveOnly);
            return GetIconForUnityName(unityName, isForPositiveOnly);
        }

        private string GetConfigKey(string unityName, bool isForPositiveOnly) {
            string suffix = (isForPositiveOnly) ? "POSITIVE" : "NEGATIVE";
            return unityName + suffix;
        }

        public static void ReturnIcon(InputUiIcon icon) {
            DestroyUtility.DestroyAsNeeded(icon);
        }

        public string GetFormattedText(string formatted) {
            int close = -1;
            int open = -1;
            do {
                if (close >= 0 && open >= 0) {
                    string contained = formatted.Substring(open + 2, close - (open + 2));
                    string[] parts = contained.Split('-');
                    PuzButton parsed = (PuzButton)Enum.Parse(typeof(PuzButton), parts[0]);
                    string descriptive = GetDescriptiveText(parsed, parts[1]);
                    formatted = formatted.Substring(0, open) + descriptive + formatted.Substring(close + 2);
                }
                open = formatted.IndexOf("[[", StringComparison.Ordinal);
                close = formatted.IndexOf("]]", StringComparison.Ordinal);
            } while (close >= 0 && open >= 0);
            return formatted;
        }

        private string GetDescriptiveText(PuzButton parsed, string tense) {
            string value = string.Empty;
            bool isForPositive;
            string unityName =  PuzButtonController.GetActiveUnityInputName(parsed, out isForPositive);
            if (unityName != null) {
                string key = GetConfigKey(unityName, isForPositive);
                if (configMap.ContainsKey(key)) {
                    InputUiButtonConfig config = configMap[key];
                    string aAction;
                    if (tense.Equals("present") || tense.Equals("terse")) {
                        aAction = config.isAxis ? "push" : "click";
                    }
                    else if (tense.Equals("holdPresent")) {
                        aAction = "hold down";
                    }
                    else if (tense.Equals("holdTerse")) {
                        aAction = "Hold Down";
                    }
                    else if (tense.Equals("holdActive")) {
                        aAction = "holding down";
                    }
                    else {
                        aAction = config.isAxis ? "pushing" : "clicking";
                    }
                    bool isTerse = tense.ToUpper().Contains("TERSE");
                    string aName = isTerse ? config.displayText : config.descriptiveText;
                    string inject = isTerse ? " " : " the ";
                    value = aAction + inject + aName;
                    if (isTerse) {
                        value = value.Substring(0, 1).ToUpper() + value.Substring(1);
                    }
                }
            }
            return value;

        }

        public string GetGroupingName(PuzButtonGroupingType aType) {
            string groupName = "Unknown";
            if (groupingConfigMap.ContainsKey(aType)) {
                groupName = groupingConfigMap[aType].displayName;
            }
            return groupName;
        }
    }
}
