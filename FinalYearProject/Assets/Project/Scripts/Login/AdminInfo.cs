using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdminInfo : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Button button;

    private void Awake()
    {
        button.onClick.AddListener(delegate { FindObjectOfType<Login>().DeleteUser(nameText.text); } );
    }
}