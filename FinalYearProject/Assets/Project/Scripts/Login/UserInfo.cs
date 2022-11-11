using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInfo : MonoBehaviour
{
    public TextMeshProUGUI text;


    public void SelectUserInfo()
    {
        FindObjectOfType<Login>().inputField.text = text.text;
        GameObject.Find("ListPanel").SetActive(false);
    }
}