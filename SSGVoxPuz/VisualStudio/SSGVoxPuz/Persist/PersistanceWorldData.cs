using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEngine;

namespace SSGVoxPuz.Persist {
    
    [Serializable]
    public class PersistanceWorldData {

        public PersistanceWorldDataPoint[] dataPoints;
        public float zoomLevel;
        public PersistanceVector3 rotation;
        public PersistanceVector3 position;
        [OptionalField] public byte[] screenShot;
        [OptionalField] public PersistanceScreenShot screenshot;
        [OptionalField] public long time;

        public void PrintInfo() {
            Debug.Log("[" + rotation.PrintInfo() +" " + position.PrintInfo() + " [" + zoomLevel + "]]");
        }
    }
}
