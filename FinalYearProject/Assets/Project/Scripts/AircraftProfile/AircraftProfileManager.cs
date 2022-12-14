using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AircraftProfileManager : MonoBehaviour
{
    public GameObject profileInfoPrefab;
    public Transform contentTransform;
    public TextAsset textJSON;
    public TMP_InputField inputField;

    [System.Serializable]
    public class Profile
    {
        public string name;
        public string aircraftName;
        public float aircraftSpeed;
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
        string i = File.ReadAllText(Application.streamingAssetsPath + "/Profiles.txt");
        list = JsonUtility.FromJson<ProfileList>(i);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SaveProfile()
    {
        string i = File.ReadAllText(Application.streamingAssetsPath + "/Profiles.txt");
        list = JsonUtility.FromJson<ProfileList>(i);

        if (list == null)
            return;

        foreach (Profile profile in list.profiles)
        {
            if (profile.name == inputField.text)
                return;
        }
        

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlanePath"))
        {
            Vector3[] positions = new Vector3[10000];
            int size = go.GetComponentInChildren<LineRenderer>().GetPositions(positions);

            Array.Resize<Vector3>(ref positions, size);
            List<SerializableVector3> serializableVector3s = new List<SerializableVector3>();
            foreach (Vector3 v in positions)
                serializableVector3s.Add(v);

            Profile profile = new Profile();
            profile.name = inputField.text;
            profile.aircraftName = go.GetComponentInChildren<AircraftInfo>().aircraftName;
            profile.aircraftSpeed = go.GetComponentInChildren<PlaneMovement>().movementSpeed;
            profile.positions = serializableVector3s;
            list.profiles.Add(profile);
        }
        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(Application.streamingAssetsPath + "/Profiles.txt", json);
    }

    public void LoadProfile()
    {
        string i = File.ReadAllText(Application.streamingAssetsPath + "/Profiles.txt");
        list = JsonUtility.FromJson<ProfileList>(i);

        if (list == null)
            return;

        foreach (Profile profile in list.profiles)
        {
            if (inputField.text != profile.name)
                continue;

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
            plane.movementSpeed = profile.aircraftSpeed;
            planeManager.SpawnPlane(plane);

            cc.currentLine = null;
        }
    }

    public void RefreshAllProfiles()
    {
        foreach (ProfileInfo info in FindObjectsOfType<ProfileInfo>())
            Destroy(info.gameObject);

        string i = File.ReadAllText(Application.streamingAssetsPath + "/Profiles.txt");
        list = JsonUtility.FromJson<ProfileList>(i);

        List<string> allProfile = new List<string>();

        if (list == null)
            return;

        foreach (Profile profile in list.profiles)
        {
            if (allProfile.Contains(profile.name))
                continue;

            ProfileInfo info = Instantiate(profileInfoPrefab, contentTransform).GetComponent<ProfileInfo>();
            info.text.text = profile.name;
            allProfile.Add(profile.name);
        }
    }
}