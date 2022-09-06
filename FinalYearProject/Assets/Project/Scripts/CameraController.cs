using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Movement")]
    Vector3 rotation = new Vector3(0, 0, 0);
    public float joystickSensitivity = 1;

    [Header("Tracking")]
    bool tracking = false;
    GameObject objectGazed = null;
    GameObject objectGazedTracked = null;
    [SerializeField] float timeToLoseTarget = 0;
    float timer = 0;
    [SerializeField] float spotRadius = 1f;

    [Header("Zoom")]
    int currZoom = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        joystickSensitivity *= 100;
        timer = timeToLoseTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if(!tracking)
        {
            CameraRotation();
        }
        else
        {
            FollowingTarget();
        }

        if (objectGazed || tracking)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ToggleTargeting();
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            CameraZooming();
        }

        if(Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if(GetComponent<ThermalController>())
                GetComponent<ThermalController>().ToggleInfrared(0);
        }

        GettingTarget();
    }

    private void CameraRotation()
    {
        float x = 0;
        float y = 0;

        float angleX = transform.rotation.eulerAngles.x;
        angleX = (angleX > 180) ? angleX - 360 : angleX;
        float currAngleX = -angleX;

        if (Input.GetJoystickNames() != null)
        {
            x = Input.GetAxis("Joystick X") * joystickSensitivity;
            y = Input.GetAxis("Joystick Y") * joystickSensitivity;
        }

        if(currAngleX <= -45 && y < 0)
        {
            rotation = new Vector3(45, transform.rotation.eulerAngles.y, 0);
        }
        else if (currAngleX >= 85 && y > 0)
        {
            rotation = new Vector3(-85, transform.rotation.eulerAngles.y, 0);
        }
        else
            rotation += new Vector3(-y, x, 0) * Time.deltaTime;

        transform.rotation = Quaternion.Euler(rotation);
    }

    private void CameraZooming()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            if (currZoom < 2)
            {
                ++currZoom;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (currZoom > 0)
            {
                --currZoom;
            }
        }

        switch (currZoom)
        {
            case 1:
                GetComponent<Camera>().fieldOfView = 45; //x2
                break;
            case 2:
                GetComponent<Camera>().fieldOfView = 10; //x9
                break;
            default:
                //40 Initialy
                GetComponent<Camera>().fieldOfView = 90;
                break;

        }
    }
    private void GettingTarget()
    {
        if (objectGazed)
            Debug.Log(objectGazed.name);
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, spotRadius, transform.forward,32000);

        if (!ObjectInRayArray(hit, objectGazed))
        {
            objectGazed = null;
        }

        foreach (RaycastHit q in hit)
        {
            Debug.DrawLine(transform.position, q.transform.position, Color.green);

            if (!Physics.Linecast(q.transform.position, transform.position))
            {
                if (objectGazed != q.transform.gameObject)
                {
                    if (objectGazed == null)
                        objectGazed = q.transform.gameObject;

                    else
                    {
                        HeatController object1Heat = objectGazed.GetComponent<HeatController>();
                        HeatController object2Heat = q.transform.gameObject.GetComponent<HeatController>();
                        if (!object1Heat && object2Heat)
                        {
                            objectGazed = q.transform.gameObject;
                        }
                        if (object1Heat && object2Heat)
                        {
                            if(object1Heat.GetHeatValue() < object2Heat.GetHeatValue())
                                objectGazed = q.transform.gameObject;
                        }
                    }
                }
            }
        }
    }
    private void ToggleTargeting()
    {
            if (tracking)
            {
                rotation = transform.rotation.eulerAngles;
                timer = timeToLoseTarget;
                objectGazedTracked = null;
                tracking = false;
                Debug.Log("Letting go a target");
            }
            else
            {
                objectGazedTracked = objectGazed;
                tracking = true;
                Debug.Log("Locking on a target");
            }
    }
    private void FollowingTarget()
    {
        Vector3 toRotate = objectGazedTracked.transform.position - transform.position;

        toRotate.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(toRotate);

        //Turning
        float smooth = 15f;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smooth * Time.deltaTime);

        //Maintaining line of sight
        if (!Physics.Linecast(objectGazedTracked.transform.position, transform.position))
            timer = timeToLoseTarget;
        else //Loses sight of target
        {
            timer -= Time.deltaTime;
        }
        
        if (timer < 0 || !objectGazedTracked || !objectGazedTracked.activeInHierarchy)
        {
            rotation = transform.rotation.eulerAngles;
            objectGazedTracked = null;
            tracking = false;
            timer = timeToLoseTarget;
        }
    }

    private bool ObjectInRayArray(RaycastHit[] array, GameObject gameObject)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].transform == gameObject) return true;
        }
        return false;
    }

    public bool isTracking()
    {
        return tracking;
    }
}
