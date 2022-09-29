using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaneManager : MonoBehaviour
{
    //Minimap
    public static PlaneManager Instance;
    public GameObject planePrefab;
    public GameObject planeIconPrefab;

    public GameObject markerPrefab;
    public GameObject markerCanvas;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.N))
        //{
        //    SpawnPlane();
        //}

        if (Input.GetKeyDown(KeyCode.B))
            markerCanvas.SetActive(false);

        else if (Input.GetKeyUp(KeyCode.B))
            markerCanvas.SetActive(true);
    }

    public void SpawnPlane()
    {
        PlaneMovement plane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity).GetComponent<PlaneMovement>();
        plane.destinations = FindObjectOfType<CursorControllerV2>().currentLine.GetComponent<LineController>().points;

        GameObject planeIcon = Instantiate(planeIconPrefab, Vector3.zero, Quaternion.identity);
        planeIcon.transform.parent = plane.transform;

        GameObject marker = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
        marker.GetComponent<PlaneWaypoint>().SetTarget(plane.transform);
        marker.transform.SetParent(markerCanvas.transform);

        FindObjectOfType<CursorControllerV2>().currentLine = null;
    }



    
}
