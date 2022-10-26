using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotCameraController : MonoBehaviour
{
    [SerializeField] float fovReducer;
    Camera currCamera;
    float originalFarClipPlane;
    // Start is called before the first frame update
    void Start()
    {
        currCamera = GetComponent<Camera>();
        currCamera.fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / fovReducer;
        originalFarClipPlane = currCamera.farClipPlane;
        //originalFarClipPlane = transform.parent.GetComponent<Camera>().farClipPlane / 24;
        //currCamera.farClipPlane = originalFarClipPlane;

    }

    public void UpdateFOV()
    {
        currCamera.fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / fovReducer;
    }

    public void UpdateClipping(int magnification)
    {
        if (magnification == 1)
            currCamera.farClipPlane = originalFarClipPlane * magnification;
        else if (magnification == 4)
            currCamera.farClipPlane = 800000;
        //currCamera.farClipPlane = originalFarClipPlane * 3f;
        else if (magnification == 24)
            currCamera.farClipPlane = 1200000;
        //currCamera.farClipPlane = originalFarClipPlane * 24;

    }
}
