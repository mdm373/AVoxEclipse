using System;
using System.Collections.Generic;
using SSGCore.Custom;
using SSGCore.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SSGVoxPuz.PuzMenu {

    
    
    
    public class PuzMenuOptionComp : CustomBehaviour, PuzMenuInteractable {

        public event Action<PuzMenuOptionHandlerComp> OnHoverStart;
        public event Action<PuzMenuOptionHandlerComp> OnHoverEnd;

        public Transform rootUp;
        public Transform iconAnchor;
        public SpriteRenderer backgroundSprite;
        public AnimationCurve selectionAnim;
        
        private PuzMenuOptionHandlerComp handlerComp;
        private PuzMenuOptionConfig config;
        private GameObject icon;
        private PuzMenuOptionLookup lookUp;
        private IPointerClickHandler field;
        private Coroutine activeCoroutine;
        private Vector3 startScale;


        public void Init(PuzMenuOptionHandlerComp aHandlerComp, PuzMenuOptionConfig aConfig, PuzMenuOptionLookup aLookUp) {
            handlerComp = aHandlerComp;
            config = aConfig;
            lookUp = aLookUp;
            GetComponentInChildren<PuzMenuOptionColliderComp>().Init(this);
            if (aHandlerComp.IconPrefab != null) {
                AllocateIcon();
            }
            if (backgroundSprite != null) {
                backgroundSprite.color = config.backgroundNormal;
            }
            UiFocus();
        }

        private  void UiFocus() {
            if (field != null) {
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                field.OnPointerClick(eventData);
            }
        }

        private void AllocateIcon() {
            icon = Instantiate(handlerComp.IconPrefab);
            TransformUtility.ChildAndNormalize(iconAnchor, icon.transform);
            handlerComp.HandleOptionInit(icon, lookUp);
            field = gameObject.GetComponentInChildren<IPointerClickHandler>();
        }

        public void UpdateIcon() {
            if (!CompUtil.IsNull(icon)) {
                DestroyUtility.DestroyAsNeeded(icon);
            }
            AllocateIcon();
        }

        public void HandleHoverStart() {
            if (config.isHoverable && handlerComp.isSelectable) {
                if (config.hoverSound != null) {
                    AudioSource.PlayClipAtPoint(config.hoverSound, transform.position);                   
                }
                if (backgroundSprite != null) {
                    backgroundSprite.color = config.backgroundHover;
                }
                if (OnHoverStart != null) {
                    OnHoverStart(handlerComp);
                }
            }
        }

        public void HandleHoverEnd() {
            if (config.isHoverable && handlerComp.isSelectable) {
                if (backgroundSprite != null) {
                    backgroundSprite.color = config.backgroundNormal;
                }
                if (OnHoverEnd != null) {
                    OnHoverEnd(handlerComp);
                }
            }
        }

        public void HandleSelected() {
            if (config.isSelectable) {
                handlerComp.HandleOptionSelect();
                UiFocus();
                if (config.selectSound != null) {
                    AudioSource.PlayClipAtPoint(config.selectSound, transform.position);
                }
            }
            if (activeCoroutine != null) {
                StopCoroutine(activeCoroutine);
                transform.localScale = startScale;
            }
            if (isActiveAndEnabled) {
                activeCoroutine = StartCoroutine(SelectionCoroutine());
            }
        }

        private IEnumerator<YieldInstruction> SelectionCoroutine() {
            float startTime = Time.time;
            startScale = transform.localScale;
            while (!CompUtil.IsNull(transform)) {
                float currentTime = Time.time - startTime;
                float strength = selectionAnim.Evaluate(currentTime);
                transform.localScale = startScale*strength;
                if (currentTime > selectionAnim.length) {
                    break;
                }
                yield return  new WaitForEndOfFrame();
            }
        }
    }
}
