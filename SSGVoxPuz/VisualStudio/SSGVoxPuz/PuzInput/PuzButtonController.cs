﻿using System;
using System.Collections.Generic;
using SSGCore.Utility;

namespace SSGVoxPuz.PuzInput {
    public class PuzButtonController {



        private static PuzButtonControllerComp controller;
        

        public static void Reset() {
            controller = null;
        }

        public static string GetUnityInputName(PuzButton button, string groupingName, out bool isForPositiveOnly) {
            string value = null;
            isForPositiveOnly = false;
            PuzButtonControllerComp aComp = GetComp();
            if (!CompUtil.IsNull(aComp)) {
                value = aComp.GetUnityInputName(button, groupingName, out isForPositiveOnly);
            }
            return value;
        }

        public static string GetActiveUnityInputName(PuzButton button, out bool isForPositiveOnly) {
            string value = null;
            isForPositiveOnly = false;
            PuzButtonControllerComp aComp = GetComp();
            if (!CompUtil.IsNull(aComp)) {
                value = aComp.GetActiveUnityInputName(button, out isForPositiveOnly);
            }
            return value;
        }

        public static bool IsListiningToInput {
            get {
                bool isListiningToInput = false;
                PuzButtonControllerComp instance = GetComp();
                if (!CompUtil.IsNull(instance)) {
                    isListiningToInput = instance.IsListiningToInput;
                }
                return isListiningToInput;
            } 
            set
            {
                PuzButtonControllerComp instance = GetComp();
                if (!CompUtil.IsNull(instance)) {
                    instance.IsListiningToInput = value;
                }
            }
            
        }
        public static void AddListener(PuzButton button, PuzButtonDriverType driver, PuzButtonEventListener listener, int priority) {
            PuzButtonControllerComp instance = GetComp();
            if (!CompUtil.IsNull(instance)) {
                instance.AddListener(driver, button, listener, priority);
            }
        }

        public static void AddListener(PuzButton button, PuzButtonDriverType driver, PuzButtonEventListener listener) {
            AddListener(button, driver, listener, 0);
        }

        public static void RemoveListener(PuzButton button, PuzButtonDriverType driver, PuzButtonEventListener listener) {
            PuzButtonControllerComp instance = GetComp();
            if (!CompUtil.IsNull(instance)) {
                instance.RemoveListener(driver, button, listener);
            }
        }

        private static PuzButtonControllerComp GetComp() {
            if (CompUtil.IsNull(controller)) {
                controller = PuzButtonControllerComp.GetSceneLoadInstance();
            }
            return controller;
        }

        public static void FireEvent(PuzButtonEventData anEvent) {
            PuzButtonControllerComp instance = GetComp();
            if (!CompUtil.IsNull(instance)) {
                instance.FireEvent(anEvent);
            }
        }

        public static List<PuzButtonGrouping> GetGroupings(PuzInputHandedness handedness, bool includeGamepad) {
            List<PuzButtonGrouping> groupings = new List<PuzButtonGrouping>();
            PuzButtonControllerComp instance = GetComp();
            if (!CompUtil.IsNull(instance)) {
                groupings = instance.GetGroupings(handedness, includeGamepad);
            }
            return groupings;
        }

        public static PuzButtonGroupingType GetActiveGroupingType() {
            PuzButtonGroupingType grouping = PuzButtonGroupingType.GamePadLeft;
            PuzButtonControllerComp instance = GetComp();
            if (!CompUtil.IsNull(instance)) {
                grouping = instance.GetActiveGroupingType();
            }
            return grouping;
        }

        public static PuzInputHandedness GetActiveGroupingHandedness() {
            PuzInputHandedness handedness = PuzInputHandedness.Right;
            PuzButtonControllerComp instance = GetComp();
            if (!CompUtil.IsNull(instance)) {
                handedness = instance.GetActiveGroupingHandedness();
            }
            return handedness;
        }
    }

}
