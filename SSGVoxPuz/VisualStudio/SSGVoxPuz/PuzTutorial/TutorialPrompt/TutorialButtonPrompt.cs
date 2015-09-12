using System;
using System.Collections;
using System.Collections.Generic;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.PuzFont;
using SSGVoxPuz.PuzInput;
using SSGVoxPuz.PuzInput.PuzInputUI;
using SSGVoxPuz.PuzMenu;
using SSGVoxPuz.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzTutorial.TutorialPrompt {
    public class TutorialButtonPrompt : CustomBehaviour {
        public event Action<TutorialButtonPrompt> OnPromptConfirmed;

        public Text displayText;
        public Transform iconAnchor;
        public AudioClip confirmSound;
        private InputUiIcon icon;
        private PuzButton button;
        public Transform viewBase;
        public Animator anim;
        public float emphasisDelay;
        public string emphasisAnimName = "Emphasis";
        public QuickLoadItemList loadList;
        private Coroutine activeCoroutine;
        private bool isDisplayed;

        public void Display(Vector2 hudPosition, Vector3 offset, string text, PuzButton aButton, bool shouldListen) {
            isDisplayed = true;
            if (loadList != null) {
                loadList.Load();
            }
            
            viewBase.localPosition = offset;
            button = aButton;
            HudController.GetSceneInstance().ShowHudItem(gameObject, hudPosition);
            icon = InputUiController.GetSceneLoadInstance().GetIcon(button);
            TransformUtility.ChildAndNormalize(iconAnchor, icon.transform);
            displayText.text = text;
            displayText.font = PuzFontController.GetSceneLoadInstance().GetFont(PuzFontType.Standard);
            LayerUtil.RelayerAll(gameObject, gameObject.layer);
            if (shouldListen) {
                StartCoroutine(ListenToPress());
            }
            if (anim != null) {
                activeCoroutine = StartCoroutine(EmphasisCoroutine());
            }
        }

        private IEnumerator<YieldInstruction> EmphasisCoroutine() {
            while (isDisplayed) { 
                yield return  new WaitForSeconds(emphasisDelay);
                anim.Play(emphasisAnimName);
            }
        }

        public IEnumerator<YieldInstruction> ListenToPress() {
            yield return  new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            PuzButtonController.AddListener(button, PuzButtonDriverType.ShortPress, HandleButtonPressed, -1);
            yield return null;
        }

        private void HandleButtonPressed(PuzButtonEventData eventdata) {
            if (eventdata.eventType == PuzButtonEventType.PressConfirmed && eventdata.isActualKeyStroke) {
                AudioSource.PlayClipAtPoint(confirmSound, transform.position);
                eventdata.isConsumed = true;
                if (OnPromptConfirmed != null) {
                    OnPromptConfirmed(this);
                }
            }
        }

        public void Close() {
            isDisplayed = false;
            if(loadList != null)
            {
                loadList.Unload();
            }
            if (activeCoroutine != null) {
                StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }
            PuzButtonController.RemoveListener(button, PuzButtonDriverType.ShortPress, HandleButtonPressed);
            HudController.GetSceneInstance().RemoveHudItem(gameObject);
            InputUiController.ReturnIcon(icon);
        }

    }
}
