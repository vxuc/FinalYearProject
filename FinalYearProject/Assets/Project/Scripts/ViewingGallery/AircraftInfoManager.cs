using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AircraftInfoManager : MonoBehaviour
{
    public TextAsset textJSON;
    public static AircraftInfoManager Instance;
    [SerializeField] Transform aircraftInfoContent;
    [SerializeField] GameObject aircraftInfoPanelPrefab;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI distanceText2;

    public GameObject currentAircraft;
    public GameObject prevAircraft;

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
                {
                    string name = "";
                    for (int i = 0; i < property.Name.Length; i++)
                    {
                        if (i == 0)
                            name += property.Name[i].ToString().ToUpper();
                        else
                        {
                            if (Char.IsUpper(property.Name[i]))
                                name += " ";

                            name += property.Name[i];
                        }
                    }
                    specs += name + ": " + property.GetValue(aircraft) + "\n";
                }
            }
            newInfo.description = aircraft.description + specs;
        }
    }

    private void Update()
    {
        distanceText.text = ((int)GameObject.Find("ViewCamera").GetComponent<Camera>().fieldOfView*12).ToString() + "m";
        distanceText2.text = ((int)GameObject.Find("ViewCamera").GetComponent<Camera>().fieldOfView*12).ToString() + "m";
    }

    public void ChangeAircraft(string name)
    {
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
        if (currentAircraft)
        {
            if (prevAircraft)
                Destroy(prevAircraft);
            prevAircraft = currentAircraft;
            prevAircraft.layer = 15;
            for (int i = 0; i < prevAircraft.transform.childCount; i++)
            {
                prevAircraft.transform.GetChild(i).gameObject.layer = 15;
            }
        }

        currentAircraft = newGO;
        currentAircraft.layer = 14;
        for (int i = 0; i < currentAircraft.transform.childCount; i++)
        {
            currentAircraft.transform.GetChild(i).gameObject.layer = 14;
        }
    }
}

