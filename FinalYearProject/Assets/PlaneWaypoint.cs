using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlaneWaypoint : MonoBehaviour
{
    public Image img;
    public Transform target;
    public TextMeshProUGUI meter;
    
    
    void Update()
    {
        if (img.transform.position.y <= 0 || img.transform.position.x <= 0)
        {
            img.enabled = false;
        }
        else
        {
            img.enabled = true;
        }

        img.transform.position = Camera.main.WorldToScreenPoint(target.position);
        Debug.DrawLine(img.transform.position, Camera.main.transform.position);
        img.transform.position = new Vector3(img.transform.position.x, img.transform.position.y + 100f, 1f);
        if (img.transform.position.y <= 0 || img.transform.position.x <= 0)
            img.transform.position = new Vector3(img.transform.position.x, 500f, 1f);

        meter.text = Vector3.Distance(target.position, transform.position).ToString("F0") + "m";
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
