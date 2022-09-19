using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerCameraController : MonoBehaviour
{
    public Camera mainCamera;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    GetComponent<Camera>() = mainCamera;
    //}

    // Update is called once per frame
    void Update()
    {
        GetComponent<Camera>().fieldOfView = mainCamera.fieldOfView;
        transform.rotation = mainCamera.transform.rotation;
        transform.position = mainCamera.transform.position;
    }
}
