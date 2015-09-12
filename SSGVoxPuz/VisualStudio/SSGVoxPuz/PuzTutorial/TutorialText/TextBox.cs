using System;
using System.Collections;
using System.Collections.Generic;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.PuzGlobal.PuzFont;
using SSGVoxPuz.PuzInput.PuzInputUI;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.PuzTutorial.TutorialText {
    
    public abstract class TextBox : CustomBehaviour{
        
        public Text textField;
        public GameObject attentionAnchor;
        private Animator anim;
        private string popAnimation = "PopIn";
        private int fontSize = 40;
        private Coroutine activeCoroutine;

        public string Text { set; get; }

        public GeneralTextBoxConfig GeneralConfig { set; protected get; }

        public GameObject GameObject { get { return gameObject; } }


        public event Action<TextBox> OnFinished;

        public bool IsDisplayed { get; private set; }
        
        public int FontSize {set {  fontSize  = value; }}
    

        protected abstract void DisplayExtended();
        protected abstract void CloseExtended();

        public void Display(Vector2 offset, bool isCentered) {
            if (isCentered) {
                textField.alignment = TextAnchor.MiddleCenter;
            }
            textField.font = PuzFontController.GetSceneLoadInstance().GetFont(PuzFontType.Standard);
            textField.fontSize = fontSize;
            HudController.GetSceneInstance().ShowHudItem(gameObject, offset);
            IsDisplayed = true;
            if (attentionAnchor != null) {
                attentionAnchor.SetActive(false);
                activeCoroutine = StartCoroutine(DelayShowAttention());
            }
            DisplayExtended();
        }

        private IEnumerator<YieldInstruction> DelayShowAttention() {
            yield return new WaitForSeconds(AttentionDelay);
            attentionAnchor.SetActive(true);
            activeCoroutine = null;
        }

        public float AttentionDelay { get; set; }

        public void Close() {
            if (!CompUtil.IsNull(activeCoroutine)) {
                StopCoroutine(activeCoroutine);
            }
            IsDisplayed = false;
            CloseExtended();
            FireFinished();
        }
        
        protected void FireFinished() {
            if (OnFinished != null) {
                OnFinished(this);
            }
        }

        public void Shake() {
            if (anim != null) {
                anim.SetTrigger("Shake");
            }
        }

        public void OnEnable() {
            anim = gameObject.GetComponent<Animator>();
        }
        
        public void SetBreathing(bool isBreathing) {
            if (anim != null) {
                anim.SetBool("Breathing", isBreathing);    
            }
        }

        protected string GetFormattedText() {
            InputUiController controller = InputUiController.GetSceneLoadInstance();
            return controller.GetFormattedText(Text);
        }

        

        public void WinkClosed() {
            if (anim != null) {
                anim.SetTrigger("WinkOut");
            }
        }

        public void PopIn() {
            anim.Play(popAnimation);
        }
    }
}
