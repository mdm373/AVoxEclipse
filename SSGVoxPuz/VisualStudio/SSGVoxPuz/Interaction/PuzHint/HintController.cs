using System.Collections.Generic;
using System.Linq;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGHud;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzInput;
using SSGVoxPuz.PuzInput.PuzInputUI;
using SSGVoxPuz.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace SSGVoxPuz.Interaction.PuzHint {
    class HintController : SceneSingletonQuickLoadItem<HintController>, CustomUpdater, QueueItemHandler, HintFace, PuzInteractable {
        public List<HintConfigEntry> configs;
        public Collider hintCollider;
        public AnimationCurve fadeCurve;
        public List<SpriteRenderer> toFade;
        public List<Graphic> toFadeText;
        public Transform spriteAnchor;
        public QuickLoadItemList toLoad;
        public Text groupingTitleText;
        public Text descriptionText;
        public TextAsset descriptionAsset;
        public Transform viewIndc;
        public PuzInteractableStateConfig stateConfig;

        private TransformQueue transformQueue;
        private Transform activeTransform;
        private Transform rootTransform;
        private bool wasColliding;
        private bool isColliding;
        private float fadeStartTime;
        private readonly Dictionary<PuzButtonGroupingType, HintConfigEntry> configMap = new Dictionary<PuzButtonGroupingType, HintConfigEntry>();
        private PuzButtonGroupingType oldActiveGrouping;
        private SpriteRenderer sprite;
        private InputUiController inputUiController;
        private float offset;
        private float fadeValue;
        private PuzInteractableStateManager stateManager;
        private bool isHintEnabled = true;
        private bool isInteractionEnabled = true;


        public override void Load() {
            PuzController puzController = PuzController.GetSceneLoadInstance();
            if (puzController != null) {
                puzController.Faces.Hint = this;
            }
            offset = viewIndc.localPosition.x;
            inputUiController = InputUiController.GetSceneLoadInstance();
            SceneCustomDelegator.AddUpdater(this);
            transformQueue = new TransformQueue();
            transformQueue.OnQueueFrontChange += HandleQueueChange;
            HudController.GetSceneInstance().ShowHudItem(gameObject, Vector2.zero);
            rootTransform = InteractorSelectorRootComp.GetSceneInstance().transform;
            fadeStartTime = float.MinValue;
            PopulateConfigMap();
            toLoad.Load();
            stateManager = new PuzInteractableStateManager(stateConfig, this);
            stateManager.Load();
        }

        private void PopulateConfigMap() {
            configMap.Clear();
            for (int i = 0; i < configs.Count; i++) {
                configMap[configs[i].activeType] = configs[i];
            }
            HandleGroupingChange(PuzButtonController.GetActiveGroupingType());
        }

        private void HandleQueueChange(TransformQueue queue, Transform queuehead) {
            activeTransform = queuehead;
        }

        public override void Unload() {
            stateManager.Unload();
            transformQueue.OnQueueFrontChange -= HandleQueueChange;
            SceneCustomDelegator.RemoveUpater(this);
            toLoad.Unload();
        }

        public void DoUpdate() {
            PuzButtonGroupingType currentActiveGrouping  = PuzButtonController.GetActiveGroupingType();
            if (currentActiveGrouping != oldActiveGrouping) {
                HandleGroupingChange(currentActiveGrouping);
            }
            wasColliding = isColliding;
            if (activeTransform != null) {
                Ray viewRay = new Ray(rootTransform.position, activeTransform.position - rootTransform.position);
                RaycastHit hitInfo;
                isColliding = hintCollider.Raycast(viewRay, out hitInfo, 99999);
            }
            bool isCollisionStopped = !isColliding && wasColliding;
            bool isCollisionStarted = !wasColliding && isColliding;
            if (isCollisionStarted || isCollisionStopped) {
                if (fadeValue < 1 && wasColliding) {
                    fadeStartTime = float.MinValue;
                }
                else {
                    fadeStartTime = Time.time;
                }
            }
            float currentFadeTime = Time.time - fadeStartTime;
            fadeValue = fadeCurve.Evaluate(currentFadeTime);
            if (!isColliding) {
                fadeValue = 1 - fadeValue;
            }
            for (int i = 0; i < toFade.Count; i++) {
                Color fadedColor = toFade[i].color;
                fadedColor.a = fadeValue;
                toFade[i].color = fadedColor;
            }
            for (int i = 0; i < toFadeText.Count; i++) {
                Color fadedColor = toFadeText[i].color;
                fadedColor.a = fadeValue;
                toFadeText[i].color = fadedColor;
            }
            if (!CompUtil.IsNull(sprite)) {
                Color fadedColor = sprite.color;
                fadedColor.a = fadeValue;
                sprite.color = fadedColor;
            }
            
        }

        private void HandleGroupingChange(PuzButtonGroupingType currentActiveGrouping) {
            if (configMap.Any()) {
                PuzInputHandedness handedNess = PuzButtonController.GetActiveGroupingHandedness();
                float sign = handedNess == PuzInputHandedness.Left ? 1 : -1;
                Vector3 local = viewIndc.transform.localPosition;
                local.x = offset*sign;
                viewIndc.transform.localPosition = local;

                string formatted = inputUiController.GetFormattedText(descriptionAsset.text);
                descriptionText.text = formatted;
                string groupingName = inputUiController.GetGroupingName(currentActiveGrouping);
                groupingTitleText.text = groupingName;
                if (!CompUtil.IsNull(sprite)) {
                    DestroyUtility.DestroyAsNeeded(sprite.gameObject);
                }
                oldActiveGrouping = currentActiveGrouping;
                if (configMap.ContainsKey(currentActiveGrouping)) {
                    HintConfigEntry config = configMap[currentActiveGrouping];
                    GameObject instanced = Instantiate(config.spritePrefab.gameObject);
                    sprite = instanced.GetComponent<SpriteRenderer>();
                    TransformUtility.ChildAndNormalize(spriteAnchor, sprite.transform);
                    LayerUtil.RelayerAll(spriteAnchor.gameObject, gameObject.layer);
                }
            }
        }

        public void EnqueueItem(Transform item) {
            if (transformQueue != null) {
                transformQueue.Enqueue(item);
            }
        }

        public void DequeueItem(Transform item) {
            if (transformQueue != null) {
                transformQueue.Dequeue(item);
            }
        }

        public void EnableHints() {
            isHintEnabled = true;
            UpdateVisibleState();
        }

        public void DisableHints() {
            isHintEnabled = false;
            UpdateVisibleState();
        }

        private void UpdateVisibleState() {
            viewIndc.gameObject.SetActive(isHintEnabled && isInteractionEnabled);
        }

        public bool IsInteractionEnabled { get { return isInteractionEnabled; } set { isInteractionEnabled = value; UpdateVisibleState(); } }
        
        public void HaultInteraction() {
            
        }

        public void ResetInteraction() {
            
        }
    }
}
