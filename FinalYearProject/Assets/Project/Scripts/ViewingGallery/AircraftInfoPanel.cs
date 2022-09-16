using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AircraftInfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button button;
    public string aircraftName;
    public string prefabName;

    private void Start()
    {
        button.onClick.AddListener(delegate { AircraftInfoManager.Instance.ChangeAircraft(prefabName); });
        nameText.text = aircraftName;
    }
}
