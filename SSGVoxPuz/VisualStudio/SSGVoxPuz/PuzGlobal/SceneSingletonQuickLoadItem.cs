using SSGCore.Custom;

namespace SSGVoxPuz.PuzGlobal {
    public abstract class SceneSingletonQuickLoadItem<T> : QuickLoadItem where T : CustomBehaviour {


        public static T GetSceneLoadInstance() {
            return FindObjectOfType<T>();
        }
    }
}
