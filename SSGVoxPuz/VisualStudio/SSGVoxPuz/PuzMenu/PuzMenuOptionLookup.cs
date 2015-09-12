using UnityEngine;

namespace SSGVoxPuz.PuzMenu {
    public interface PuzMenuOptionLookup {

        T FindMenuOption<T>(string id) where T : PuzMenuOptionHandlerComp;
        void ChangeSubTitle(string title);
        GameObject GetOptionsRoot();
    }
}
