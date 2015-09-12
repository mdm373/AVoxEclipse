using SSGVoxPuz.Persist;
using SSGVoxPuz.PuzTutorial.TutorialText;
using UnityEditor;
using UnityEngine;

namespace SSGVoxPuzEditor.Persist {
    
    [CustomEditor(typeof(PersistanceController))]
    class PersistanceControllerEditor : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            PersistanceController controller = (PersistanceController) target;
            if (Application.isPlaying) {
                if(GUILayout.Button("Load Asset"))
                {
                    controller.LoadWorld(controller.toLoad);
                }
            }
        }
    }
}
