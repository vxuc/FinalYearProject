using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DetailsPage : MonoBehaviour
{
    [Header("Close Button")]
    [SerializeField] GameObject pageToClose;

    [Header("Details Text")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI heightText;

    //Current Plane
    GameObject plane;

    public void Update()
    {
        //PlaneWaypoint plane = new PlaneWaypoint();

        if (plane != null)
        {
            Debug.Log(plane.name);
           
            float camToPlaneDist = Vector3.Distance(plane.transform.position, Camera.main.transform.position) / 100000;
            distanceText.text = camToPlaneDist.ToString("Distance: " + "0") + "km";
            heightText.text = (plane.transform.position.y / 30.48).ToString("Height: " + "0") + "ft";
        }

    }
    public void SetPlane(GameObject Plane)
    {
        this.plane = Plane;
    }


    //Close the page
    public void closePage()
    {
        Destroy(pageToClose);
    }

    //Delete a plane on minimap
    public void deletePlane()
    {
        //Delete Plane, Lines, Icons
    }
}
