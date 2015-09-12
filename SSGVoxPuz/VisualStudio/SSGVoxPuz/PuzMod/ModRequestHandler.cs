using System.Collections.Generic;
using SSGCore.Utility;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using UnityEngine;

namespace SSGVoxPuz.PuzMod {
    
    class ModRequestHandler {

        private readonly VoxelBlockVolume targetWorld;
        private readonly Dictionary<ushort, VoxelSingleBlockWorldIndividualCache> caches;
        private readonly ModHandlerConfig config;
        
        private ModMaterialAlphaHandlerComp alphaHandler;
        private ModControllerRequest request;
        private VoxelWorldComp activeWorld;
        private bool isBusy;
        private Vector3 destination;
        private Vector3 start;
        private float startTime;
        private bool isAir;
        private ushort activeWorldType;
        
        public ModRequestHandler(
            VoxelBlockVolume world, 
            Dictionary<ushort, VoxelSingleBlockWorldIndividualCache> worldCaches,
            ModHandlerConfig modConfig) 
        {
            targetWorld = world;
            caches = worldCaches;
            config = modConfig;
        }

        public bool IsBusy() {
            return isBusy;
        }

        public void StartHandling(ModControllerRequest requested) {
            isAir = VoxelBlockRegistry.GetAirType().ByteCode == requested.blockType.ByteCode;
            bool isOpValid = true;
            if (isAir) {
                uint blockCount = targetWorld.GetBlockCount();
                if (blockCount == 1) { // Dont Allow User To Set Last Block To Air
                    isBusy = false;
                    isOpValid = false;
                }

            }

            if (isOpValid) {
                startTime = Time.time;
                isBusy = true;
                request = requested;
                HandleValidatedStart();
            }
            

            

        }

        private void HandleValidatedStart() {
            activeWorldType = isAir ? request.existingBlockType.ByteCode : request.blockType.ByteCode;

            VoxelSingleBlockWorldIndividualCache cache = caches[activeWorldType];
            activeWorld = cache.GetCachedItem();

            alphaHandler = activeWorld.GetComponentInChildren<ModMaterialAlphaHandlerComp>();
            alphaHandler.SetMaterial(config.modelMaterial);

            TransformUtility.ChildAndNormalize(targetWorld.GetTransform(), activeWorld.transform);
            Vector3 variance = Random.insideUnitSphere * config.variance;

            if (!isAir) {
                destination = request.position.GetVector();
                activeWorld.transform.localPosition = destination + variance;
                start = activeWorld.transform.localPosition;
            }
            else {
                targetWorld.SetBlockType(request.position, request.blockType);
                activeWorld.transform.localPosition = request.position.GetVector();
                activeWorld.transform.parent = null;
                start = activeWorld.transform.position;
                destination = start + variance;
                Rigidbody activeBody = activeWorld.GameObject.AddComponent<Rigidbody>();
                Vector2 airVariance = Random.insideUnitCircle * config.airVariance;
                Vector3 airDirection = new Vector3(airVariance.x, 1, airVariance.y);
                airDirection.Normalize();
                Vector3 velocity = airDirection * config.airSpeed;
                Vector3 angularVelocity = Random.insideUnitSphere * config.airSpin;
                activeBody.angularVelocity = angularVelocity;
                activeBody.velocity = velocity;
            }
        }

        public void DoUpdate() {
            if (isBusy) {
                
                float time = Time.time;
                float percent = (time - startTime) / config.speed;
                if (!isAir) {
                    alphaHandler.SetAlpha(percent);
                }
                else {
                    alphaHandler.SetAlpha(1-percent);
                }
                if (!isAir) {
                    activeWorld.transform.localPosition = Vector3.Lerp(start, destination, percent);
                }
                if (percent >= 1) {
                    Hault(true);
                }
            }
        }

        public void Hault(bool isBlockApplied) {
            isBusy = false;
            if (!isAir) {
                if (isBlockApplied) {
                    targetWorld.SetBlockType(request.position, request.blockType);
                }
            }
            else {
                DestroyUtility.DestroyAsNeeded(activeWorld.GetComponent<Rigidbody>());
            }
            caches[activeWorldType].ReturnCacheItem(activeWorld);
        }
    }
}
