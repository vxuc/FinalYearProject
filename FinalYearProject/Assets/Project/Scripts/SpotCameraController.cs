using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotCameraController : MonoBehaviour
{
    [SerializeField] float fovReducer;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / fovReducer;
    }

    public void UpdateFOV()
    {
        GetComponent<Camera>().fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / fovReducer;
    }
}
