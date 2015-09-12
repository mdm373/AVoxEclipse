using System;

namespace SSGVoxPuz.Persist {
    
    [Serializable]
    public class PersistanceScreenShot {

        public byte[] pngBytes;
        public int height;
        public int width;
    }
}
