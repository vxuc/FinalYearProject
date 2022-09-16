using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftInfoManager : MonoBehaviour
{
    public static AircraftInfoManager Instance;
    public List<GameObject> aircrafts = new List<GameObject>();
    [SerializeField] Transform aircraftInfoContent;
    [SerializeField] GameObject aircraftInfoPanelPrefab;

    public GameObject currentAircraft;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in aircrafts)
        {
            AircraftInfoPanel newInfo = Instantiate(aircraftInfoPanelPrefab, aircraftInfoContent).GetComponent<AircraftInfoPanel>();
            newInfo.aircraftName = go.GetComponent<AircraftInfo>().aircraftName;
            newInfo.prefabName = go.GetComponent<AircraftInfo>().prefabName;
        }
    }

    public void ChangeAircraft(string name)
    {
        if (currentAircraft != null)
            Destroy(currentAircraft);

        foreach (GameObject go in aircrafts)
        {
            if (go.GetComponent<AircraftInfo>().prefabName == name)
            {
                GameObject newGO = Instantiate(go, Vector3.zero, Quaternion.identity);
                newGO.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                currentAircraft = newGO;
            }
        }
    }
}
