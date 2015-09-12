using SSGVoxPuz.Interaction;

namespace SSGVoxPuz.Pivot {
    class PivotPointAdjustGestureComp : QueueItemComp{
        public override QueueItemHandler GetHandler() {
            return PivotPointAdjustControllerComp.GetSceneLoadInstance();
        }
    }
}
