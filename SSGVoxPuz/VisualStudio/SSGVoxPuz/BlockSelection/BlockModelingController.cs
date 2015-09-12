using System;
using System.Collections.Generic;
using System.Linq;
using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using UnityEngine;

namespace SSGVoxPuz.BlockSelection {
    public class BlockModelingController : SceneSingletonQuickLoadItem<BlockModelingController>, BlockTypeFace {

        public event Action<ushort> OnActiveBlockTypeChange;
        
        public VoxelBlockMetaDataEntryIntegerState textureIndex;
        public QuickLoadItem hudItemPrefab;

        [SerializeField] private GameObject iconPrefab;
        [SerializeField] private ushort activeBlockType;
        private PuzController puzController;
        private QuickLoadItem hudItem;

        public GameObject IconPrefab { get { return iconPrefab; } set { iconPrefab = value; } }

        public override void Load() {
            
            GameObject hudtemObject = Instantiate(hudItemPrefab.gameObject);
            hudItem = hudtemObject.GetComponent<QuickLoadItem>();
            TransformUtility.ChildAndNormalize(transform, hudtemObject.transform);
            hudItem.Load();
            puzController = PuzController.GetSceneLoadInstance();
            puzController.OnReset += HandleReset;
            puzController.Faces.BlockType = this;
        }

        public override void Unload() {
            hudItem.Unload();
            puzController.OnReset -= HandleReset;
        }

        private void HandleReset(PuzController obj) {
            ActiveBlockType = ModelBlockGenerator.GetSceneLoadInstance().baseIndex;
        }



        public ushort ActiveBlockType {
            get {
                return activeBlockType;
            }
            set {
                activeBlockType = value;
                if (OnActiveBlockTypeChange != null) {
                    OnActiveBlockTypeChange(activeBlockType);
                }
            }
        }
        
        

        public List<ushort> GetModelingBlocks() {
            List<ushort> registeredBlocks = VoxelBlockRegistry.GetCurrentlyRegisteredBlockTypeCodes().ToList();
            List<ushort> modelingBlocks = new List<ushort>();
            for (int i = 0; i < registeredBlocks.Count; i++) {
                ushort blockCode = registeredBlocks[i];
                VoxelBlock block = VoxelBlockRegistry.GetBlock(blockCode);
                bool isModelable = block.BlockMetaData.Get(ModelingConstants.META_DATA_KEY, false);
                if (isModelable) {
                    modelingBlocks.Add(blockCode);
                }
            }
            return modelingBlocks;
        }

        public void EnableBlockTypeChange() {
            hudItem.gameObject.SetActive(true);
        }

        public void DisableBlockTypeChange() {
            hudItem.gameObject.SetActive(false);
        }
    }
}
