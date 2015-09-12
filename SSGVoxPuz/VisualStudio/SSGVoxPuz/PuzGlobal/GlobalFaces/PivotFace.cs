using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface PivotFace {
        void EnablePivoting();
        void DisablePivoting();
        void SetPosition(Vector3 position);
        Vector3 GetPosition();
    }
}
