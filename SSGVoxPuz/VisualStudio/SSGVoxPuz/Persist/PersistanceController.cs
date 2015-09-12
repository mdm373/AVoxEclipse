using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using SSGVoxel.APIS;
using SSGVoxel.Core;
using SSGVoxPuz.Interaction;
using SSGVoxPuz.Persist.Screenshot;
using SSGVoxPuz.PuzGlobal;
using SSGVoxPuz.PuzGlobal.GlobalFaces;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    public class PersistanceController : SceneSingletonQuickLoadItem<PersistanceController>, PersistanceFace {
        public TextAsset toLoad;
        
        private string fileSavePath;
        private bool isSaving;

        public event Action<PersistanceFace> OnSave;
        public event Action<PersistanceFace> OnDelete;

        public void LoadWorld(TextAsset asset) {
            PersistanceWorldData data = LoadTextAsset(asset);
            SetWorldToData(data);
        }

        public void LoadWorld(string saveName) {
            InteractionGlobalControllerComp.GetSceneLoadInstance().ResetAll();
            string loadPath = GetFileSavePath(saveName);
            PersistanceWorldData data = LoadFile(loadPath);
            SetWorldToData(data);

        }

        public void SaveWorld(string saveName) {
            if (!isSaving) {
                isSaving = true;
                fileSavePath = GetFileSavePath(saveName);
                ScreenshotController screenShotcontroller = ScreenshotController.GetSceneLoadInstance();
                screenShotcontroller.OnScreenshotTaken += HandleScreenshotTaken;
                screenShotcontroller.RequestScreenshot();
            }
        }

        private void HandleScreenshotTaken(ScreenshotController arg1, Texture2D arg2) {
            PersistanceWorldData data = GetPersistanceData();
            data.screenshot = new PersistanceScreenShot {
                pngBytes = arg2.EncodeToPNG(),
                height = arg2.height,
                width = arg2.width
            };
            WriteFile(data, fileSavePath);
            if (OnSave != null) {
                OnSave(this);
            }
            isSaving = false;
        }

        public static string[] GetAllSaves() {
            return Directory.GetFiles(GetSaveLocation(), "*.voxModel");
        }

        private static string GetSaveLocation() {
            return PersistanceGlobalsController.GetSceneInstance().GetSaveLocation();
        }

        public static string GetFileSavePath(string saveName) {
            return GetSaveLocation() + Path.DirectorySeparatorChar + saveName + ".voxModel";
        }

        private static PersistanceWorldData LoadTextAsset(TextAsset asset) {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(asset.bytes);
            PersistanceWorldData data = (PersistanceWorldData)bf.Deserialize(stream);
            stream.Close();
            return data;
        }

        public static PersistanceWorldData LoadFile(string loadPath) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(loadPath, FileMode.Open);
            PersistanceWorldData data = (PersistanceWorldData)bf.Deserialize(fs);
            fs.Close();
            return data;
        }

        private static void WriteFile(PersistanceWorldData data, string saveFilePath) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream ms = new FileStream(saveFilePath, FileMode.Create);
            bf.Serialize(ms, data);
            Debug.Log("Model Data Saved To: " + saveFilePath);
            ms.Close();
        }

        private static PersistanceWorldData GetPersistanceData() {
            Debug.Log("Building Persistance Data");
            PuzController controller = PuzController.GetSceneLoadInstance();
            Dictionary<VoxelWorldPosition, VoxelBlockType> allBlocks = controller.GetPuzzleWorld().World.GetAllBlocks();
            List<VoxelWorldPosition> positions = allBlocks.Keys.ToList();
            int positionCount = positions.Count;
            PersistanceWorldData data = new PersistanceWorldData {
                dataPoints = new PersistanceWorldDataPoint[positionCount]
            };
            data.zoomLevel = controller.Faces.Zoom.GetZoomLevel();
            data.position = PersistanceVector3.FromVector3(controller.Faces.Pivot.GetPosition());
            data.rotation = PersistanceVector3.FromVector3(controller.Faces.Rotate.GetRotation());
            

            for (int positionIndex = 0; positionIndex < positionCount; positionIndex++) {
                VoxelWorldPosition position = positions[positionIndex];
                PersistanceWorldDataPoint dataPoint = new PersistanceWorldDataPoint {
                    x = position.X,
                    y = position.Y,
                    z = position.Z,
                    code = allBlocks[position].ByteCode
                };
                data.dataPoints[positionIndex] = dataPoint;
            }
            data.PrintInfo();
            data.time = DateTime.Now.Ticks;
            return data;
        }
        
        private static void SetWorldToData(PersistanceWorldData data) {
            Debug.Log("Setting World To Data");
            data.PrintInfo();
            PuzController controller =  PuzController.GetSceneLoadInstance();
            VoxelBlockVolume world = controller.GetPuzzleWorld().World;
            controller.Faces.Zoom.SetZoomLevel(data.zoomLevel);
            controller.Faces.Pivot.SetPosition(data.position.ToVector3());
            controller.Faces.Rotate.SetRotation(data.rotation.ToVector3());

            PersistanceWorldDataPoint[] dataPoints = data.dataPoints;
            List<VoxelWorldPosition> toRemove =world.GetAllBlocks().Keys.ToList();
            for (int i = 0; i < dataPoints.Count(); i++) {
                PersistanceWorldDataPoint dataPoint = dataPoints[i];
                VoxelWorldPosition worldPos = new VoxelWorldPosition(dataPoint.x, dataPoint.y, dataPoint.z);
                if (toRemove.Contains(worldPos)) {
                    toRemove.Remove(worldPos);
                }
                VoxelBlockType type = VoxelBlockRegistry.GetBlockType(dataPoint.code);
                world.SetBlockType(worldPos, type);
            }
            VoxelBlockType airType = VoxelBlockRegistry.GetAirType();
            for (int i = 0; i < toRemove.Count; i++) {
                world.SetBlockType(toRemove[i], airType);
            }

        }

        public override void Load() {
            PuzController.GetSceneLoadInstance().Faces.Persist = this;
        }

        public override void Unload() {
            PuzController.GetSceneLoadInstance().Faces.Persist = null;
        }

        public void DeleteWorld(string modelName) {
            string path = GetFileSavePath(modelName);
            File.Delete(path);
            if (OnDelete != null) {
                OnDelete(this);
            }
        }
    }
}
