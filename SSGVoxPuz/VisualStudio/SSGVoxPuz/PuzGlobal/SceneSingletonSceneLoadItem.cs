using SSGCore.Custom;

namespace SSGVoxPuz.PuzGlobal {
    public abstract class SceneSingletonSceneLoadItem<T> : SceneLoadItem where T : CustomBehaviour {

        public static T GetSceneLoadInstance() {
            return FindObjectOfType<T>();
        }
    }
}
