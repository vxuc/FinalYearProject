using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaneManager : MonoBehaviour
{
    [Header("Plane Icon")]
    //Minimap
    public static PlaneManager Instance;
    //public GameObject planePrefab;
    public GameObject planeIconPrefab;

    [Header("Plane Selection")]
    //Plane Selection Prefabs
    public List<GameObject> planePrefabs = new List<GameObject>();

    [Header("Marker")]
    public GameObject markerPrefab;
    public GameObject markerCanvas;

    public int spawnCount = 0;
    public int planeVal = 0;

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

    public void SpawnPlane(int val)
    {
        if(spawnCount < 16)
        {
            if (planeVal != 0 && FindObjectOfType<CursorControllerV2>().currentLine)
            {
                if (FindObjectOfType<CursorControllerV2>().currentLine.points.Count >= 2)
                {

                    spawnCount++;

                    CursorControllerV2 cursorController = FindObjectOfType<CursorControllerV2>();
                    Debug.Log(val);
                    //SELECTION STATEMENT
                    if (planeVal <= 0)
                    {
                        Debug.Log("CHOOSE A PLANE TO SPAWN");
                    }
                    else
                    {
                        //Plane
                        PlaneMovement plane = Instantiate(planePrefabs[planeVal - 1], Vector3.zero, Quaternion.identity, cursorController.GetPlanePathParent()).GetComponent<PlaneMovement>();
                        SpawnPlane(plane);
                    }

                    cursorController.currentLine = null;
                }
            }
        }

        else
        {
         Debug.Log("MAX SPAWN COUNT REACHED");
        }
    }

    public void SpawnPlane(PlaneMovement plane)
    {
        plane.destinations = FindObjectOfType<CursorControllerV2>().currentLine.GetComponent<LineController>().points;

        //Plane Icon 
        GameObject planeIcon = Instantiate(planeIconPrefab, new Vector3(0, 100000, 0), Quaternion.Euler(new Vector3(0, 180, 0)), plane.transform);

        //Waypoint Marker
        GameObject marker = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity, markerCanvas.transform);
        marker.GetComponent<PlaneWaypoint>().SetTarget(plane.transform);
    }

    public void PlaneSelectionValue(int val)
    {
        planeVal = val;
    }
    
}
