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

    [Header("Deleting Plane")]
    [SerializeField] GameObject planePathParent;

    //Current Plane
    public GameObject plane;

    public void Update()
    {
        //PlaneWaypoint plane = new PlaneWaypoint();

        if (plane != null)
        {
            Debug.Log(plane.name);        
            heightText.text = (plane.transform.position.y / 30.48).ToString("Height: " + "0") + "ft";
        }

    }
    public void SetPlane(GameObject Plane)
    {
        this.plane = Plane;
    }

    public void MertricConverter(int val)
    {
        float camToPlaneDist = Vector3.Distance(plane.transform.position, Camera.main.transform.position) / 100000;

        //KM
        if (val == 1)
        {
            distanceText.text = camToPlaneDist.ToString("Distance: " + "0") + "km";
        }

        //NM
        if(val == 2)
        {
            distanceText.text = (camToPlaneDist / 1.852).ToString("Distance: " + "0") + "nm";
        }
    }



    //Close the page
    public void closePage()
    {
        Destroy(pageToClose);
    }

    //Delete a plane on minimap
    public void deletePlane()
    {
        ReplayManager.Instance.DestroyRecordedGO(plane.transform.parent.transform.parent.gameObject);
        Destroy(pageToClose);
    }
}
