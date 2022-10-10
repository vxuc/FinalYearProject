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
        public string type;
        public string prefab;
        public int scale;
        public string description;

        public string category1;
        public string wingMount;
        public string dihedral;
        public string taper;
        public string tip;

        public string category2;
        public string nose;
        public string cockpit;
        public string midSection;
        public string rearSection;

        public string category3;
        public string engineNumber;
        public string engineType;
        public string engineMount;
        public string intakeNumber;
        public string intakeShape;
        public string intakeLocation;
        public string exhaustNumber;
        public string exhaustLocation;

        public string category4;
        public string shape;
        public string tailFin;
        public string tailPlane;
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
            newInfo.type = aircraft.type;

            string specs = "";
            foreach (var property in aircraft.GetType().GetFields())
            {
                if (property.Name.Contains("category"))
                    specs += "\n" + property.GetValue(aircraft).ToString().ToUpper() + "\n";
                else
                    specs += property.Name + ": " + property.GetValue(aircraft) + "\n";
            }
            newInfo.description = aircraft.description + specs;
        }
    }

    public void ChangeAircraft(string name)
    {
        if (currentAircraft != null)
            Destroy(currentAircraft);

        if (Resources.Load("Models/" + name) == null)
            return;

        int scale = 0;
        foreach (Aircraft aircraft in list.aircraft)
        {
            if (aircraft.prefab == name)
            {
                scale = aircraft.scale;
            }
        }

        GameObject newGO = Instantiate(Resources.Load("Models/" + name) as GameObject, Vector3.zero, Quaternion.identity);
        newGO.transform.localScale = new Vector3(0.001f * scale, 0.001f * scale, 0.001f * scale);
        currentAircraft = newGO;
    }
}

