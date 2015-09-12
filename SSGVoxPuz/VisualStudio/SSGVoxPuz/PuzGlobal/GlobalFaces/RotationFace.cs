using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface RotationFace {
        void EnableRoation();
        void DisableRotation();
        void SetRotation(Vector3 roation);
        Vector3 GetRotation();
    }
}
