using System;
using System.Collections.Generic;
using System.Linq;
using SSGCore.Custom;
using SSGCore.Utility;
using SSGVoxel.BlockBounds;
using SSGVoxel.Core;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using SSGVoxPuz.PuzMod;
using UnityEngine;

namespace SSGVoxPuz.Tools {
    class PuzToolController : SceneSingletonQuickLoadItem<PuzToolController>, CustomUpdater, ToolFace {

        public event Action<PuzTool> OnActiveItemChange;

        [SerializeField] private PuzTool activeTool;
        [SerializeField] private List<PuzToolConfig> toolConfigs;
        [SerializeField] private PuzToolHandlerBubbleConfig bubbleConfig;
        [SerializeField] private GameObject previewWorldPrefab;
        [SerializeField] private QuickLoadItem hudItemPrefab;

        private Dictionary<PuzTool, PuzToolHandler> toolHandlers;
        private List<PuzToolHandler> toolHandlerList = new List<PuzToolHandler>();
        private VoxelWorldComp previewWorld;
        private PuzModificationController modController;
        private PuzController puzController;
        private QuickLoadItem hudItem;
        private bool isToolsEnabled;

        public override void Load() {
            isToolsEnabled = true;
            GameObject hutItemObject = Instantiate(hudItemPrefab.gameObject);
            hudItem = hutItemObject.GetComponent<QuickLoadItem>();
            TransformUtility.ChildAndNormalize(transform, hudItem.transform);
            hudItem.Load();
            modController = PuzModificationController.GetSceneLoadInstance();
            AllocatePreviewWorld();
            SceneCustomDelegator.AddUpdater(this);
            toolHandlers = GetToolHandlers();
            
            puzController = PuzController.GetSceneLoadInstance();
            puzController.OnReset += HandlePuzReset;
            puzController.Faces.Tools = this;

        }

        public override void Unload() {
            hudItem.Unload();
            puzController.OnReset -= HandlePuzReset;
        }

        private void HandlePuzReset(PuzController obj) {
            ActiveTool = PuzTool.Pencil;
        }

        private void AllocatePreviewWorld() {
            if (!CompUtil.IsNull(previewWorld)) {
                DestroyUtility.DestroyAsNeeded(previewWorld.GameObject);
            }
            GameObject previewObject = Instantiate(previewWorldPrefab);
            TransformUtility.ChildAndNormalize(transform, previewObject.transform);
            previewWorld = previewObject.GetComponentInChildren<VoxelWorldComp>();
            previewWorld.GameObject.SetActive(false);
        }

        private Dictionary<PuzTool, PuzToolHandler> GetToolHandlers() {
            Dictionary<PuzTool, PuzToolHandler> handlers = new Dictionary<PuzTool, PuzToolHandler>();
            handlers[PuzTool.Pencil] = new PuzToolHandlerPencil(modController);
            handlers[PuzTool.Eraser] = new PuzToolHandlerEraser(modController);
            handlers[PuzTool.Brush] = new PuzToolHandlerBrush(modController);
            handlers[PuzTool.Bubble] = new PuzToolHandlerBubble(this, bubbleConfig, previewWorld, modController);
            toolHandlerList = handlers.Values.ToList();
            return handlers;
        }

        public PuzToolHandlerBubbleConfig BubbleDragConfig { get { return bubbleConfig;} set { bubbleConfig = value; } }
        public GameObject PreviewWorldPrefab { get { return previewWorldPrefab; } set { previewWorldPrefab = value; } }
        public QuickLoadItem HudItemPrefab { get { return hudItemPrefab; } set { hudItemPrefab = value; } }

        public PuzTool ActiveTool {
            get { return activeTool; }
            set {
                activeTool = value;
                if (OnActiveItemChange != null) {
                    OnActiveItemChange(activeTool);
                }
            }
        }

        public List<PuzToolConfig> ToolConfigs {
            get { return toolConfigs; }
            set { toolConfigs = value; }
        }
        

        public GameObject GetToolIconPrefab(PuzTool tool) {
            PuzToolConfig matchConfig = GetConfig(tool);
            GameObject prefab = null;
            if (matchConfig.tool != PuzTool.Unknown) {
                prefab = matchConfig.iconPrefab;
            }
            return prefab;
        }

        public string GetDescription(PuzTool tool) {
            PuzToolConfig matchConfig = GetConfig(tool);
            string description = string.Empty;
            if (matchConfig.tool != PuzTool.Unknown) {
                description = matchConfig.description;
            }
            return description;
        }

        public List<PuzTool> GetActiveTools() {
            List<PuzTool> tools = new List<PuzTool>();
            for (int i = 0; i < toolConfigs.Count; i++) {
                tools.Add(toolConfigs[i].tool);
            }
            return tools;
        }

        public GameObject GetHoverPrefab(PuzTool tool) {
            GameObject hoverPrefab = null;
            for (int i = 0; i < toolConfigs.Count; i++) {
                if (toolConfigs[i].tool == tool) {
                    hoverPrefab = toolConfigs[i].hoverPrefab;
                }
            }
            return hoverPrefab;
        }

        private PuzToolConfig GetConfig(PuzTool tool) {
            PuzToolConfig config = new PuzToolConfig();
            for (int i = 0; i < toolConfigs.Count; i++) {
                if (toolConfigs[i].tool == tool) {
                    config = toolConfigs[i];
                    break;
                }
            }
            return config;
        }

        public void HandleToolRequested(BoundsHit hit, ushort block) {
            if (isToolsEnabled) {
                PuzToolHandler activeToolHandler = toolHandlers[activeTool];
                int pendingRequestCount = modController.GetPendingModificationsCount();
                if (activeToolHandler.IsEditWithPendingModificationAllowed() || pendingRequestCount == 0) {
                    if (activeToolHandler.HandleToolRequested(hit, block)) {
                        PlayModAudio(hit);
                    }
                }
            }
        }

        private void PlayModAudio(BoundsHit hit) {
            PuzToolConfig config = GetConfig(activeTool);
            AudioClip clip = config.requestSound;
            Vector3 position = GetPosition(hit);
            AudioSource.PlayClipAtPoint(clip, position);
        }

        private static Vector3 GetPosition(BoundsHit hit) {
            Vector3 localPosition = hit.WorldPosition.GetVector();
            return hit.World.GetTransform().TransformPoint(localPosition);
        }

        public void SetGesture(Transform gesture) {
            for (int i = 0; i < toolHandlerList.Count; i++) {
                toolHandlerList[i].SetGesture(gesture);    
            }
            
        }

        public void HandleSelectionCanceled() {
            if (isToolsEnabled) {
                toolHandlers[activeTool].HandleSelectionCanceled();
            }
        }

        public void DoUpdate() {
            if (isToolsEnabled) {
                for (int i = 0; i < toolHandlerList.Count; i++) {
                    toolHandlerList[i].DoUpdate();
                }
            }
        }

        public void HandlePress() {
            if (isToolsEnabled) {
                toolHandlers[activeTool].HandlePress();
            }
        }

        public void DisableTools() {
            isToolsEnabled = false;
            hudItem.gameObject.SetActive(false);
        }

        public void EnableTools() {
            isToolsEnabled = true;
            hudItem.gameObject.SetActive(true);
        }

        public void SetTool(PuzTool tool) {
            ActiveTool = tool;
        }
    }
}
