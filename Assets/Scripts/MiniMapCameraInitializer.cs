using UnityEngine;

public class MiniMapCameraInitializer : MonoBehaviour
{
    public MinimapCameraConfig config;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = config.orthographicSize;
        cam.nearClipPlane = config.nearClip;
        cam.farClipPlane = config.farClip;
        cam.rect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
    }
}
