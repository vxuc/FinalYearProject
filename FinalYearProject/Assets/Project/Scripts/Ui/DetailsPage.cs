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
    public TextMeshProUGUI bearingText;

    [Header("Deleting Plane")]
    [SerializeField] GameObject planePathParent;

    //Current Plane
    public GameObject plane;

    public void Update()
    {
        //PlaneWaypoint plane = new PlaneWaypoint();

        double planeHeight = plane.transform.position.y / 30.48;
        float bearingHeight = (Mathf.Atan2(plane.transform.position.x - Camera.main.transform.position.x, plane.transform.position.z - Camera.main.transform.position.z)) * Mathf.Rad2Deg;

        bearingHeight = (bearingHeight < 0) ? bearingHeight + 360 : bearingHeight;
        bearingHeight = (bearingHeight > 359) ? bearingHeight = 0 : bearingHeight;


        if (plane != null)
        {
            Debug.Log(plane.name);        
            heightText.text = planeHeight.ToString("Height: " + "0") + "ft";
            bearingText.text = bearingHeight.ToString("Bearing: " + "0 ") + "degrees";
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
        PlaneManager.Instance.spawnCount--;
        Debug.Log("Spawn Count DELETE: " + PlaneManager.Instance.spawnCount);
    }
}
