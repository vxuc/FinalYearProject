using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraZoom
    {
        x1,
        x4,
        x24,
        TOTAL_ZOOMS
    };

    [Header("Replay")]
    public ReplayManager replayManager;

    [Header("Camera Movement")]
    Vector3 rotation = Vector3.zero;
    [SerializeField] float joystickSensitivity = 100;
    float originalJoystickSensitivity;
    float overTimeSensitivity = 0;
    public GameObject spyder;

    bool resetting = false;
    [SerializeField] float lagTime = 0.3f;
    float inputLagTime;
    float outputLagTime;
    Vector3 currDir = Vector3.zero;

    [Header("Tracking")]
    public Camera spotCamera;//Additional Camera to get the target as raycast is smaller the longer the distance the object is from the camera
    bool tracking = false;
    bool forcedTracking = false; //After Track Ends
    GameObject objectGazed = null;
    GameObject objectGazedTracked = null;
    [SerializeField] float timeToLoseTarget = 0;
    float timer = 0;
    [SerializeField] float spotRadius = 1f;
    [SerializeField] float timeToTrackTarget = 0;
    float smoothTimer;


    [Header("Zoom")]
    float cameraOriginalFOV;
    float cameraThermalOriginalFOV;

    float magnificationFactor = 1.0f;
    float magnificationLag = 1.0f;
    bool zooming = false;
    CameraZoom cameraZoom = 0;
    float zoomRate;
    public FlashingUiController[] flashEffect;
    [SerializeField] float flashDelay = 0.7f;
    [SerializeField] float flashLerp = 0.5f;

    [Header("Thermal")]
    public ThermalController thermalController;

    [Header("Modes")]
    public InformationController informationController;


    // Start is called before the first frame update
    void Start()
    {
        //Factory setting for camera
        originalJoystickSensitivity = joystickSensitivity;
        timer = timeToLoseTarget;
        smoothTimer = timeToTrackTarget + 1;

        if (GetComponent<Camera>()) //For zooming
            cameraOriginalFOV = GetComponent<Camera>().fieldOfView;
        else
            cameraOriginalFOV = 70;

        cameraThermalOriginalFOV = cameraOriginalFOV * 24 / 17; //Diff Pov for thermal cam

        rotation += spyder.transform.rotation.eulerAngles; //Changes rotation on where the spyder is facing at the start

        inputLagTime = 0.15f;
        outputLagTime = lagTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!replayManager.ReplayMode())
        {
            if (informationController.GetRDRMode())
            {
                if (IsTracking()) //To stop trackings
                    ToggleTargeting();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    ToggleTargeting();
                }

                if (Input.GetKeyDown(KeyCode.Joystick1Button10) && (!tracking || !forcedTracking))
                {
                    resetting = true;
                }
            }

            //Does not matter if its RDR or not
            if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                CameraZooming();
                zooming = true;

                //Getting zoomRate
                zoomRate = (magnificationFactor - magnificationLag) * Time.fixedDeltaTime / 0.2f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!replayManager.ReplayMode())
        {
            if (!informationController.GetRDRMode() && !forcedTracking)
            {
                if (!tracking && !resetting)
                {
                    CameraRotation();
                }
                else if (tracking)
                {
                    FollowingTarget();
                }

                else if (resetting)
                {
                    ResetCameraAxis();
                }

                GettingTargetV2();
            }
            if (zooming)
            {
                CameraZoomingLag();
            }
        }
    }

    private void CameraRotation()
    {
        float x = 0;
        float y = 0;

        float angleX = transform.rotation.eulerAngles.x + spyder.transform.rotation.eulerAngles.x;//Offset default
        angleX = (angleX > 180) ? angleX - 360 : angleX;
        float currAngleX = -angleX;

        float maxOverTimeSensitivity = 5;

        if (Input.GetJoystickNames() != null)
        {

            x = Input.GetAxis("Joystick X") * joystickSensitivity * overTimeSensitivity / maxOverTimeSensitivity;
            y = Input.GetAxis("Joystick Y") * joystickSensitivity * overTimeSensitivity / maxOverTimeSensitivity;
        }
        if (Input.GetAxis("Joystick X") != 0 || Input.GetAxis("Joystick Y") != 0) //check if there is a joystick input
        {
            inputLagTime -= Time.deltaTime;
            if (inputLagTime < 0)
            {
                if (overTimeSensitivity < maxOverTimeSensitivity)
                    overTimeSensitivity += Time.deltaTime;

                if (currAngleX <= -45 && y < 0) //min elevation angle
                {
                    rotation = new Vector3(45, transform.rotation.eulerAngles.y, 0);
                }
                else if (currAngleX >= 85 && y > 0)//max elevation angle
                {
                    rotation = new Vector3(-85, transform.rotation.eulerAngles.y, 0);
                }
                else
                    rotation += new Vector3(-y, x, 0) * Time.deltaTime;

                currDir = new Vector3(-y, x, 0);
            }
        }
        else
        {
            inputLagTime = 0.15f;
            overTimeSensitivity = 0;
            outputLagTime -= Time.deltaTime;
        }
        if (outputLagTime > 0)
        {
            rotation += currDir * Time.deltaTime;
        }
        else
        {
            outputLagTime = lagTime;
            currDir = Vector3.zero;
        }

        transform.rotation = Quaternion.Euler(rotation);
    }

    private void CameraZooming()
    {

        float currFOV = GetComponent<Camera>().fieldOfView;
        if (!replayManager.ReplayMode())
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button5)) //Zoom in
            {
                if (cameraZoom < CameraZoom.TOTAL_ZOOMS - 1)
                {
                    ++cameraZoom;
                    //Flash
                    if (flashEffect.Length > 0)
                    {
                        for (int i = 0; i < flashEffect.Length; i++)
                            flashEffect[i].StartFlash(flashLerp, flashDelay);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button3)) //Zoom out
            {
                if (cameraZoom > 0)
                {
                    --cameraZoom;

                    //Flash
                    if (flashEffect.Length > 0)
                    {
                        for (int i = 0; i < flashEffect.Length; i++)
                            flashEffect[i].StartFlash(flashLerp, flashDelay);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Joystick1Button7))//Thermal
            {
                thermalController.ChangeCameraMode();
            }
        }
        switch (cameraZoom)//Change magnification
        {
            case CameraZoom.x24:
                magnificationFactor = 24f;
                break;
            case CameraZoom.x4:
                magnificationFactor = 4f;
                break;
            default:
                magnificationFactor = 1f;
                break;

        }

        joystickSensitivity = originalJoystickSensitivity / magnificationFactor;

        //Spot Camera
        spotCamera.GetComponent<SpotCameraController>().UpdateFOV();
        spotCamera.GetComponent<SpotCameraController>().UpdateClipping((int)magnificationFactor); //Change the max range to get the target
    }

    private void CameraZoomingLag()
    {
        if (zoomRate > 0)
        {
            if (magnificationLag >= magnificationFactor)
            {
                magnificationLag = magnificationFactor;
                zooming = false;
            }
        }
        else
        {
            if (magnificationLag <= magnificationFactor)
            {
                magnificationLag = magnificationFactor;
                zooming = false;
            }
        }

        //magnificationLag += zoomRate;
        magnificationLag = magnificationFactor;

        if (thermalController)//change FOV from color to thermal vice versa
        {
            switch (thermalController.GetCameraMode())
            {
                case ThermalController.CameraModes.COLOR:
                    GetComponent<Camera>().fieldOfView = cameraOriginalFOV / magnificationLag;
                    break;
                default:
                    GetComponent<Camera>().fieldOfView = cameraThermalOriginalFOV / magnificationFactor;
                    break;
            }
        }
        else
            GetComponent<Camera>().fieldOfView = cameraOriginalFOV / magnificationLag;
    }

    private void GettingTargetV2()
    {

        Collider[] gameObjects = (Collider[])FindObjectsOfType(typeof(Collider));//Get objects with collider
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(spotCamera); //Get camera view

        if (objectGazed)//objects gazed went out of bound from camera
        {
            if (!GeometryUtility.TestPlanesAABB(planes, objectGazed.GetComponent<Collider>().bounds))
            {
                objectGazed = null;
            }
        }

        foreach (Collider gameObject in gameObjects)
        {
            if (gameObject.transform.gameObject.layer < 20 && gameObject.gameObject.layer != LayerMask.NameToLayer("SpyderBody") && gameObject.gameObject.layer != LayerMask.NameToLayer("Plane")
                && !Physics.Linecast(gameObject.transform.position, transform.position)) //Check whether the object should be trackable or not
            {
                bool onSight = GeometryUtility.TestPlanesAABB(planes, gameObject.transform.GetComponent<Collider>().bounds); // check if gameobject is on camera view

                if (onSight)
                {
                    if (objectGazed != gameObject.transform.gameObject)
                    {
                        if (objectGazed == null)
                            objectGazed = gameObject.transform.gameObject;

                        else
                        {
                            HeatController object1Heat = objectGazed.GetComponent<HeatController>();
                            HeatController object2Heat = gameObject.transform.gameObject.GetComponent<HeatController>();

                            if (!object1Heat && object2Heat) //Get the object with the highest heat
                            {
                                objectGazed = gameObject.transform.gameObject;
                            }
                            else if (object1Heat && object2Heat)
                            {
                                if (object1Heat.GetHeatValue() < object2Heat.GetHeatValue())
                                    objectGazed = gameObject.transform.gameObject;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    private void ToggleTargeting()
    {
        if (tracking) //Letting go of a target
        {
            rotation = transform.rotation.eulerAngles;
            timer = timeToLoseTarget;
            smoothTimer = timeToTrackTarget;
            objectGazedTracked = null;
            tracking = false;
        }
        else if (forcedTracking)
        {
            forcedTracking = false;
        }
        else if (objectGazed)
        {
            if (!Physics.Linecast(objectGazed.transform.position, transform.position)) //Locking on a target
            {
                objectGazedTracked = objectGazed;
                tracking = true;
            }
        }
        else
        {
            forcedTracking = true; //Forced track on sky
        }
    }
    private void FollowingTarget()
    {
        if (timer < 0 || !objectGazedTracked.activeInHierarchy || !objectGazedTracked) //Loses track of target
        {
            rotation = transform.rotation.eulerAngles;
            objectGazedTracked = null;
            tracking = false;
            forcedTracking = true;
            timer = timeToLoseTarget;
            smoothTimer = timeToTrackTarget + 1;
            return;
        }
        RaycastHit raycastHit;

        if (objectGazedTracked?.transform.Find("Pivot")) //Get the supposed area that need to be tracked
        {
            Vector3 toRotate = objectGazedTracked.transform.position 
                - transform.position;

            Vector3 cross = Vector3.Cross(transform.rotation * Vector3.forward, Quaternion.LookRotation(toRotate) * Vector3.forward);
            Debug.Log(cross.y);

            //Offset
            toRotate = objectGazedTracked.transform.Find("Pivot").position
                     + new Vector3(objectGazedTracked.transform.Find("Pivot").localPosition.x, 0, objectGazedTracked.transform.Find("Pivot").localPosition.z)
                     - transform.position;
            if (cross.y < 0)
            {
                toRotate = objectGazedTracked.transform.Find("Pivot").position
                    - new Vector3(objectGazedTracked.transform.Find("Pivot").localPosition.x, 0, objectGazedTracked.transform.Find("Pivot").localPosition.z)
                    - transform.position;
            }

            Quaternion desiredRotation = Quaternion.LookRotation(toRotate);

            //Turning
            float smooth = 50f * objectGazedTracked.GetComponentInParent<PlaneMovement>().movementSpeed / 10000;


            if (smoothTimer > 1f)
            {
                smoothTimer -= Time.deltaTime;
            }
            //this.transform.LookAt(objectGazedTracked?.transform.Find("Pivot"));
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, smooth * Time.fixedDeltaTime / smoothTimer);


            Debug.DrawLine(transform.position, objectGazedTracked.transform.Find("Pivot").position, Color.red);
            //Maintaining line of sight
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>());
            bool collided = Physics.Linecast(objectGazedTracked.transform.position, gameObject.transform.position, out raycastHit);

            if (collided)
            {
                if (GeometryUtility.TestPlanesAABB(planes, objectGazedTracked.GetComponent<Collider>().bounds) && (raycastHit.transform == objectGazedTracked.transform || raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Lamps")))
                    timer = timeToLoseTarget;

                else //Loses sight of target
                {
                    Debug.Log("Error");
                    timer -= Time.deltaTime;
                }
            }
        }
        else //No pivots
        {
            rotation = transform.rotation.eulerAngles;
            objectGazedTracked = null;
            tracking = false;
            forcedTracking = true;
            timer = timeToLoseTarget;
            smoothTimer = timeToTrackTarget + 1;
        }
    }

    private void ResetCameraAxis()
    {
        rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, spyder.transform.rotation.eulerAngles.y, 0)), 4.5f * Time.deltaTime).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation);
        Debug.Log(rotation.x + "," + rotation.y);

        float degree = 0.5f;
        if (Mathf.Abs(transform.rotation.eulerAngles.x - spyder.transform.rotation.eulerAngles.x) <= degree && Mathf.Abs(transform.rotation.eulerAngles.y - spyder.transform.rotation.eulerAngles.y) <= degree)
            resetting = false;
    }
    private bool ObjectInRayArray(RaycastHit[] array, GameObject gameObject)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].transform == gameObject) return true;
        }
        return false;
    }

    public bool IsTracking()
    {
        return tracking;
    }

    public bool isForcedTracking()
    {
        return forcedTracking;
    }

    public void SetIsTracking(bool track)
    {
        tracking = track;
    }

    public CameraZoom GetCameraZoomLevel()
    {
        return cameraZoom;
    }

    public void SetCameraZoomLevel(int level)
    {
        cameraZoom = (CameraZoom)level;
        CameraZooming();
        CameraZoomingLag();
    }

    public GameObject GetTrackedGameObject()
    {
        return objectGazedTracked;
    }

    public void SpyderDirChange()
    {
        resetting = true;
    }
}
