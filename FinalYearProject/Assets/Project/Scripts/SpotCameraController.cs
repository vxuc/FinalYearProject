using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotCameraController : MonoBehaviour
{
    [SerializeField] float fovReducer;
    Camera currCamera;
    //float originalFarClipPlane;
    // Start is called before the first frame update
    void Start()
    {
        currCamera = GetComponent<Camera>();
        currCamera.fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / fovReducer;
        //originalFarClipPlane = transform.parent.GetComponent<Camera>().farClipPlane / 24;
        //currCamera.farClipPlane = originalFarClipPlane;
        
    }

    public void UpdateFOV()
    {
        currCamera.fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / fovReducer;
    }

    public void UpdateClipping(int magnification)
    {
        currCamera.farClipPlane = 23750 * magnification;
    }
}
