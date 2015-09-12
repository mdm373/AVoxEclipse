using SSGVoxPuz.Interaction;

namespace SSGVoxPuz.PuzRotation {
    class PuzRotationBallGestureComp : QueueItemComp {
        public override QueueItemHandler GetHandler() {
            return PuzRotationBallControllerComp.GetSceneLoadInstance();
        }
    }
}
