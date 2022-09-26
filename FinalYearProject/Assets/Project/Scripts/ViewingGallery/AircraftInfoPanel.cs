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
    public string type;
    public string prefabName;
    public string description;

    private void Start()
    {
        button.onClick.AddListener(delegate { AircraftInfoManager.Instance.ChangeAircraft(prefabName); });
        button.onClick.AddListener(delegate { ChangeDescription(); });
        nameText.text = aircraftName;
    }

    public void ChangeDescription()
    {
        GameObject.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = description;
    }
}
