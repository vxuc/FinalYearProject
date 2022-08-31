using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReplayInfo : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string replayName;

    private void Start()
    {
        text.text = replayName;
    }

    public void UpdateReplayName()
    {
        DataManager.Instance.replayName = replayName;
        ReplayManager.Instance.LoadReplay();
    }
}