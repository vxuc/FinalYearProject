using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / 4;
    }

    public void UpdateFOV()
    {
        GetComponent<Camera>().fieldOfView = transform.parent.GetComponent<Camera>().fieldOfView / 4;
    }
}
