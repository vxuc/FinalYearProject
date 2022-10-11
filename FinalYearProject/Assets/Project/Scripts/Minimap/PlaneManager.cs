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

    public int spawnCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            markerCanvas.SetActive(false);

        else if (Input.GetKeyUp(KeyCode.B))
            markerCanvas.SetActive(true);

        //Spawn Counter
        Debug.Log("Spawn Count: " + spawnCount);
    }

    public void SpawnPlane()
    {
        if(spawnCount < 16 && FindObjectOfType<CursorControllerV2>().currentLine)
        {
            if (FindObjectOfType<CursorControllerV2>().currentLine.points.Count >= 2)
            {

                spawnCount++;

                CursorControllerV2 cursorController = FindObjectOfType<CursorControllerV2>();

                PlaneMovement plane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity, cursorController.GetPlanePathParent()).GetComponent<PlaneMovement>();
                plane.destinations = FindObjectOfType<CursorControllerV2>().currentLine.GetComponent<LineController>().points;

                GameObject planeIcon = Instantiate(planeIconPrefab, Vector3.zero, Quaternion.identity, plane.transform);

                GameObject marker = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity, markerCanvas.transform);
                marker.GetComponent<PlaneWaypoint>().SetTarget(plane.transform);

                cursorController.currentLine = null;
            }
            }

            else
        {
            Debug.Log("MAX SPAWN COUNT REACHED");
        }
    }



    
}
