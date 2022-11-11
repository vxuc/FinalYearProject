using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileInfo : MonoBehaviour
{
    public TextMeshProUGUI text;


    public void SelectProfileInfo()
    {
        GameObject.Find("ProfileInputField").GetComponent<TMP_InputField>().text = text.text;
        GameObject.Find("ListPanel").SetActive(false);
    }
}