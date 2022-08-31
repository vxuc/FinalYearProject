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

    public void RefreshReplays()
    {
        DirectoryInfo dir = new DirectoryInfo("Replays/");
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
}
