using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlaneWaypoint : MonoBehaviour
{
    public Image img;
    public Transform target;
    
    void Update()
    {
        img.transform.position = Camera.main.WorldToScreenPoint(target.position);
        Debug.DrawLine(img.transform.position, Camera.main.transform.position);
        img.transform.position = new Vector3(img.transform.position.x, img.transform.position.y + 100f, 1f);
    }
}
