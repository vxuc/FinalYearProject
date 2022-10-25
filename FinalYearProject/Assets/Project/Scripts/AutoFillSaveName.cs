using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class AutoFillSaveName : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    // Start is called before the first frame update
    void Awake()
    {
        //inputField.text = System.DateTime.Now.ToString();
        if (!Directory.Exists("Replays/" + PlayerPrefs.GetString("Username")))
        {
            inputField.text = PlayerPrefs.GetString("Username") + "1";
            return;
        }

        DirectoryInfo dir = new DirectoryInfo("Replays/" + PlayerPrefs.GetString("Username") + "/");
        var info = dir.GetDirectories(".");
        int count = dir.GetDirectories().Length;
        inputField.text = PlayerPrefs.GetString("Username") + (count + 1).ToString();
    }
}
