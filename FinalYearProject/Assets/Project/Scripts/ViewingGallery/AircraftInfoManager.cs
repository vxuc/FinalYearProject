using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AircraftInfoManager : MonoBehaviour
{
    public TextAsset textJSON;
    public static AircraftInfoManager Instance;
    [SerializeField] Transform aircraftInfoContent;
    [SerializeField] GameObject aircraftInfoPanelPrefab;
    [SerializeField] TextMeshProUGUI descriptionText;

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

    [System.Serializable]
    public class Aircraft
    {
        public string name;
        public string prefab;
        public string description;
    }

    [System.Serializable]
    public class AircraftList
    {
        public Aircraft[] aircraft;
    }

    public AircraftList list = new AircraftList();
    // Start is called before the first frame update
    void Start()
    {
        list = JsonUtility.FromJson<AircraftList>(textJSON.text);

        foreach (Aircraft aircraft in list.aircraft)
        {
            AircraftInfoPanel newInfo = Instantiate(aircraftInfoPanelPrefab, aircraftInfoContent).GetComponent<AircraftInfoPanel>();
            newInfo.aircraftName = aircraft.name;
            newInfo.prefabName = aircraft.prefab;
            newInfo.description = aircraft.description;
        }
    }

    public void ChangeAircraft(string name)
    {
        if (currentAircraft != null)
            Destroy(currentAircraft);

        if (Resources.Load("Models/" + name) == null)
            return;

        GameObject newGO = Instantiate(Resources.Load("Models/" + name) as GameObject, Vector3.zero, Quaternion.identity);
        newGO.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        currentAircraft = newGO;
    }
}

