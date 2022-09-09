using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    enum CameraZoom
    {
        x1,
        x4,
        x24,
        TotalZooms
    };

    [Header("Camera Movement")]
    Vector3 rotation = new Vector3(0, 0, 0);
    [SerializeField] float joystickSensitivity = 100;
    float originalJoystickSensitivity;

    [Header("Tracking")]
    public Camera spotCamera;
    bool tracking = false;
    GameObject objectGazed = null;
    GameObject objectGazedTracked = null;
    [SerializeField] float timeToLoseTarget = 0;
    float timer = 0;
    [SerializeField] float spotRadius = 1f;

    [Header("Zoom")]
    float cameraOriginalFOV;
    float cameraThermalOriginalFOV;

    float fMagnificationFactor = 1.0f;
    CameraZoom cameraZoom = 0;

    [Header("Thermal")]
    public ThermalController thermalController;


    // Start is called before the first frame update
    void Start()
    {
        originalJoystickSensitivity = joystickSensitivity;
        timer = timeToLoseTarget;

        if (GetComponent<Camera>())
            cameraOriginalFOV = GetComponent<Camera>().fieldOfView;
        else
            cameraOriginalFOV = 70;

        cameraThermalOriginalFOV = cameraOriginalFOV * 24 / 17;
    }

    // Update is called once per frame
    void Update()
    {

        if (objectGazed || tracking)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ToggleTargeting();
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            CameraZooming();
        }

        GettingTarget();
    }

    private void FixedUpdate()
    {
        if (!tracking)
        {
            CameraRotation();
        }
        else
        {
            FollowingTarget();
        }
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

        float currFOV = GetComponent<Camera>().fieldOfView;
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            if (cameraZoom < CameraZoom.TotalZooms - 1)
            {
                ++cameraZoom;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (cameraZoom > 0)
            {
                --cameraZoom;
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            thermalController.ChangeCameraMode();
        }

        switch (cameraZoom)
        {
            case CameraZoom.x24:
                fMagnificationFactor = 24f;
                break;
            case CameraZoom.x4:
                fMagnificationFactor = 4f;
                break;
            default:
                fMagnificationFactor = 1f;
                break;

        }

        if (thermalController)
        {
            switch (thermalController.GetCameraMode())
            {
                case ThermalController.CameraModes.Color:
                    GetComponent<Camera>().fieldOfView = cameraOriginalFOV / fMagnificationFactor;
                    break;
                default:
                    GetComponent<Camera>().fieldOfView = cameraThermalOriginalFOV / fMagnificationFactor;
                    break;
            }
        }
        else
            GetComponent<Camera>().fieldOfView = cameraOriginalFOV / fMagnificationFactor;


        joystickSensitivity = originalJoystickSensitivity / fMagnificationFactor;
        spotCamera.GetComponent<SpotCameraController>().UpdateFOV();
    }

    private void GettingTarget()
    {
        if (objectGazed)
            Debug.Log(objectGazed.name);

        RaycastHit[] hit = Physics.SphereCastAll(transform.position, spotRadius, transform.forward,float.MaxValue);
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(spotCamera);

        if (!ObjectInRayArray(hit, objectGazed))
        {
            objectGazed = null;
        }

        foreach (RaycastHit q in hit)
        {
            Debug.DrawLine(transform.position, q.transform.position, Color.green);
            bool onSight = false;

            if (q.transform.GetComponent<Renderer>())
                onSight = GeometryUtility.TestPlanesAABB(planes, q.transform.GetComponent<Renderer>().bounds); ;

            if (!Physics.Linecast(q.transform.position, transform.position) && onSight)
            {
                if (objectGazed != q.transform.gameObject)
                {
                    if (q.transform.gameObject.layer != 6)
                    {
                        if (objectGazed == null)
                            objectGazed = q.transform.gameObject;

                        else
                        {
                            HeatController object1Heat = objectGazed.GetComponent<HeatController>();
                            HeatController object2Heat = q.transform.gameObject.GetComponent<HeatController>();
                            if (!object1Heat && object2Heat && q.transform.gameObject.layer != 6)
                            {
                                objectGazed = q.transform.gameObject;
                            }
                            if (object1Heat && object2Heat)
                            {
                                if (object1Heat.GetHeatValue() < object2Heat.GetHeatValue())
                                    objectGazed = q.transform.gameObject;
                            }
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
