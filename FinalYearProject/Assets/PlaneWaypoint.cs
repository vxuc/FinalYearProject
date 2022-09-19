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

        //Positioning of marker
        if (img.transform.position.y <= 0 || img.transform.position.x <= 0)
            img.transform.position = new Vector3(img.transform.position.x, 500f, 1f);

        //Convert m to km and removed two zeros suffix
        float camToPlaneDist = Vector3.Distance(target.position, transform.position) / 100000;
        meter.text = camToPlaneDist.ToString("F0") + "km";
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
