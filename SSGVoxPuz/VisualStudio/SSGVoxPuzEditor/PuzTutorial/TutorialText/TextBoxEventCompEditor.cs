using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGVoxPuz.PuzTutorial.TutorialText;
using UnityEditor;
using UnityEngine;

namespace SSGVoxPuzEditor.PuzTutorial.TutorialText {
    
    [CustomEditor(typeof(TextBoxEventComp))]
    class TextBoxEventCompEditor : Editor {
        
        public override void OnInspectorGUI() {
            TextBoxEventComp comp = (TextBoxEventComp) target;
            DrawDefaultInspector();
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.stretchHeight = true;
            GUILayout.BeginVertical(style);
            GUILayout.Label("Display Text");
            GUIStyle boxStyle = new GUIStyle(GUI.skin.textArea);
            boxStyle.stretchHeight = true;
            comp.textToDisplay = EditorGUILayout.TextArea(comp.textToDisplay, boxStyle);
            GUILayout.EndVertical();
        }
    }
}
