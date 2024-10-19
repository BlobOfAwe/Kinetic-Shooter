using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // This script is for testing purposes.

    [SerializeField]
    private float zoomInLevel = 5f;

    [SerializeField]
    private float zoomOutLevel = 100f;

    private bool isZoomedOut = false;

    public void OnZoom()
    {
        if (isZoomedOut)
        {
            GetComponent<Camera>().orthographicSize = zoomInLevel;
            isZoomedOut = false;
        } else
        {
            GetComponent<Camera>().orthographicSize = zoomOutLevel;
            isZoomedOut = true;
        }
    }
}
