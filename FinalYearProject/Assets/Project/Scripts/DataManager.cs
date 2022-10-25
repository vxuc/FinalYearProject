using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string replayName;

    [SerializeField] GameObject replayInfoPrefab;
    [SerializeField] Transform contentTransform;

    public int id;
    public List<GameObject> prefabList = new List<GameObject>();

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

    public void SetReplayName(string name)
    {
        replayName = name;
    }

    public int GetNewID()
    {
        return id++;
    }

    public void RefreshReplays()
    {
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Destroy(contentTransform.GetChild(i).gameObject);
        }

        if (PlayerPrefs.GetInt("IsOperator") == 1)
        {
            Debug.Log("ISOP");
            OperatorReplays();
            return;
        }

        if (!Directory.Exists("Replays/" + PlayerPrefs.GetString("Username")))
            return;

        DirectoryInfo dir = new DirectoryInfo("Replays/" + PlayerPrefs.GetString("Username") + "/");
        var info = dir.GetDirectories(".");
        int count = dir.GetDirectories().Length;
        for (int i = 0; i < count; i++)
        {
            int index = info[i].ToString().Length - 1;
            string s = "";
            while (true)
            {
                if (Char.IsLetterOrDigit(info[i].ToString()[index]))
                    s += info[i].ToString()[index];
                else
                    break;

                index--;
            }

            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            s = new string(charArray);
            Debug.Log("Found Directory: " + s);

            ReplayInfo replayInfo = Instantiate(replayInfoPrefab, contentTransform).GetComponent<ReplayInfo>();
            replayInfo.replayName = s;
        }
    }

    void OperatorReplays()
    {
        DirectoryInfo dir2 = new DirectoryInfo("Replays/");
        var info2 = dir2.GetDirectories(".");
        int count2 = dir2.GetDirectories().Length;
        for (int i = 0; i < count2; i++)
        {
            int index = info2[i].ToString().Length - 1;
            string s = "";
            while (true)
            {
                if (Char.IsLetterOrDigit(info2[i].ToString()[index]))
                    s += info2[i].ToString()[index];
                else
                    break;

                index--;
            }

            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            s = new string(charArray);
            Debug.Log("Found Directory: " + s);

            DirectoryInfo dir = new DirectoryInfo("Replays/" + s + "/");
            var info = dir.GetDirectories(".");
            int count = dir.GetDirectories().Length;
            for (int j = 0; j < count; j++)
            {
                int index2 = info[j].ToString().Length - 1;
                string ss = "";
                while (true)
                {
                    if (Char.IsLetterOrDigit(info[j].ToString()[index2]))
                        ss += info[j].ToString()[index2];
                    else
                        break;

                    index2--;
                }

                char[] charArray2 = ss.ToCharArray();
                Array.Reverse(charArray2);
                ss = new string(charArray2);
                Debug.Log("Found Directory: " + ss);

                ReplayInfo replayInfo = Instantiate(replayInfoPrefab, contentTransform).GetComponent<ReplayInfo>();
                replayInfo.replayName = ss;
            }
        }
    }
}
