using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> panels;

    public void OpenPanel(string name)
    {
        foreach (GameObject panel in panels)
        {
            if (panel.name == name)
            {
                panel.SetActive(!panel.activeSelf);
            }
            else
                panel.SetActive(false);
        }
    }
}
