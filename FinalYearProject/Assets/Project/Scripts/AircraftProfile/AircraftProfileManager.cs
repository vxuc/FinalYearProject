using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System;

public class AircraftProfileManager : MonoBehaviour
{
    public Transform transformParent;
    public TextAsset textJSON;
    public TMP_InputField inputField;

    [System.Serializable]
    public class Profile
    {
        public string name;
        public string aircraftName;
        public List<SerializableVector3> positions;
    }

    [System.Serializable]
    public class ProfileList
    {
        public List<Profile> profiles = new List<Profile>();
    }

    public ProfileList list = new ProfileList();

    // Start is called before the first frame update
    void Start()
    {
        list = JsonUtility.FromJson<ProfileList>(textJSON.text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveProfile()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlanePath"))
        {
            Vector3[] positions = new Vector3[10000];
            int size = go.GetComponentInChildren<LineRenderer>().GetPositions(positions);

            Array.Resize<Vector3>(ref positions, size);
            List<SerializableVector3> serializableVector3s = new List<SerializableVector3>();
            foreach (Vector3 v in positions)
                serializableVector3s.Add(v);

            Profile profile = new Profile();
            profile.name = "Jordan";
            profile.aircraftName = go.GetComponentInChildren<AircraftInfo>().aircraftName;
            profile.positions = serializableVector3s;
            list.profiles.Add(profile);
        }
        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(Application.dataPath + "/Project/Resources/Profiles.txt", json);
    }

    public void LoadProfile()
    {
        list = JsonUtility.FromJson<ProfileList>(textJSON.text);
        foreach (Profile profile in list.profiles)
        {
            CursorControllerV2 cc = FindObjectOfType<CursorControllerV2>();
            Transform t = Instantiate(cc.PlanePathParent.gameObject, Vector3.zero, Quaternion.identity).transform;
            
            LineController lc = cc.renderLine(true, t);

            foreach (Vector3 v in profile.positions)
            {
                GameObject go = Instantiate(cc.prefab, lc.transform.parent);
                go.transform.position = v;
                lc.AddPoint(go.transform);
            }

            PlaneManager planeManager = FindObjectOfType<PlaneManager>();
            PlaneMovement plane = Instantiate(FindObjectOfType<PlaneManager>().planePrefabs.Find(GameObject => GameObject.name == profile.aircraftName), t).GetComponent<PlaneMovement>();
            planeManager.SpawnPlane(plane);

            cc.currentLine = null;
        }
    }
}