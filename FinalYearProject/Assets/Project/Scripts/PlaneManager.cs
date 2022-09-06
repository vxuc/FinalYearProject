using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    public static PlaneManager Instance;
    public GameObject planePrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            SpawnPlane();
        }
    }

    public void SpawnPlane()
    {
        Instantiate(planePrefab, Vector3.zero, Quaternion.identity);
    }

    
}
