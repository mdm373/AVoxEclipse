namespace SSGVoxPuz.PuzGlobal.GlobalFaces {
    public interface ZoomFace {
        void EnableZoom();
        void DisableZoom();
        float GetZoomLevel();
        void SetZoomLevel(float zoomLevel);
        void HaultActiveZooming();
    }
}
