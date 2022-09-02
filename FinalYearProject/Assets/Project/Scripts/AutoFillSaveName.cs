using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoFillSaveName : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    // Start is called before the first frame update
    void Awake()
    {
        inputField.text = System.DateTime.Now.ToString();
    }
}
