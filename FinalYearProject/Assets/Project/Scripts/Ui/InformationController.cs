using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationController : MonoBehaviour
{
    public Transform userCamera;
    CameraController cameraController;
    public GameObject spyder;

    [Header("Ui")]
    public TextMeshProUGUI rotationText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI modesText;
    public Image trackingDots;
    public Image pinkDot;
    public Image pinkSpike;

    [Header("Rotation")]
    public RectTransform rotationIndicator;
    public RectTransform spyderIndicator;
    bool trueNorth = true;
    public TextMeshProUGUI northText;

    [Header("ModesText")]
    bool firstHalf = true; //TC/RDR
    bool secondHalf = true; //Central/Autonomous

    bool error = false;
    string toSay = "Error";
    float errorTimer = 3;
    float spikeTimer = 3;

    [Header("CrossHair")]
    public Image Border;

    [Header("Zoom")]
    public TextMeshProUGUI zoomText;
    CameraController.CameraZoom currZoomLevel;

    public System.DateTime dateTime = System.DateTime.Now;

    System.DateTime startTime;

    void Start()
    {
        modesText.text = "TC | Central";

        if (userCamera.GetComponent<CameraController>())
        {
            cameraController = userCamera.GetComponent<CameraController>();
            currZoomLevel = cameraController.GetCameraZoomLevel();
        }
        startTime = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationText && userCamera)
            updateRotationText();
        if (timeText)
            updateTimeText();

 

        if (modesText)
        {
            if ((Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.Joystick1Button8)) && !error)
            {
                UpdateModes();
            }
        }

        if (pinkDot && cameraController)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                DisplayPinkDot();
            }
            if((!firstHalf && secondHalf) && pinkDot.gameObject.activeSelf)
            {
                pinkDot.gameObject.SetActive(false);
            }
            
            if (pinkDot.gameObject.activeSelf)
            {
                spikeTimer -= Time.deltaTime;
                if (spikeTimer <= 0)
                    pinkSpike.gameObject.SetActive(true);

            }
            else
            {
                spikeTimer = 3;
                pinkSpike.gameObject.SetActive(false);
            }
        }

        if(trackingDots && cameraController)
        {
            DisplayTrackingDots();
        }

        if (error)
        {
            if (errorTimer > 0)
            {
                errorTimer -= Time.deltaTime;
            }
            else
            {
                error = false;
                errorTimer = 3;
                modesText.color = Color.white;
                UpdateModesText();
            }
        }

        if(Border)
        {
            DisplayBorder();
        }

        if(zoomText)
        {
            if(currZoomLevel != cameraController.GetCameraZoomLevel())
            {
                UpdateZoomText();
                currZoomLevel = cameraController.GetCameraZoomLevel();
            }
        }
    }

    private void updateTimeText()
    {
        if (ReplayManager.Instance.ReplayMode())
        {
            System.TimeSpan timeDiff = startTime - dateTime;
            string time = System.DateTime.Now.Subtract(timeDiff).ToString("HH:mm:ss");
            string date = System.DateTime.Now.Subtract(timeDiff).ToString("dd/MM/yy");
            timeText.text = date + "    " + time;
        }
        else
        {
            string time = System.DateTime.Now.ToString("HH:mm:ss");
            string date = System.DateTime.Now.ToString("dd/MM/yy");
            timeText.text = date + "    " + time;
        }
    }

    private void updateRotationText()
    {
        float az = userCamera.transform.rotation.eulerAngles.y;
        if(!trueNorth)
            az = userCamera.transform.rotation.eulerAngles.y - spyder.transform.rotation.eulerAngles.y;

        if(rotationIndicator)
            rotationIndicator.localRotation = Quaternion.Euler(new Vector3(0, 0, -userCamera.transform.rotation.eulerAngles.y));
        if (spyderIndicator)
            spyderIndicator.localRotation = Quaternion.Euler(new Vector3(0, 0, -spyder.transform.rotation.eulerAngles.y));

        az = (az < 0) ? az + 360: az;
        az = (az > 359) ? az = 0 : az;
            //string az = (userCamera.rotation.y * Mathf.Rad2Deg).ToString("F0");
            //string el = userCamera.rotation.eulerAngles.x.ToString("F1");
            float angle = userCamera.transform.rotation.eulerAngles.x - spyder.transform.rotation.eulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;
        float el = -angle;
        if(el > 0)
            rotationText.text = "TL :\nAZ : +" + az.ToString("F0") + "°\nEL : +" + el.ToString("F1") + "°";
        else
            rotationText.text = "TL :\nAZ : +" + az.ToString("F0") + "°\nEL : " + el.ToString("F1") + "°";
    }

    private void UpdateModes()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button9) && secondHalf)
        {
            if (firstHalf)
                firstHalf = false;
            else
                firstHalf = true;
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button9) && !secondHalf)
        {
            error = true;
            toSay = " Can't Comply - RDR";
        }

        if(Input.GetKeyDown(KeyCode.Joystick1Button8) && firstHalf)
        {
            if (secondHalf)
                secondHalf = false;
            else
                secondHalf = true;
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button8) && !firstHalf)
        {
            error = true;
            toSay = " Can't Comply - Autonomous";
        }
        UpdateModesText();
    }

    private void UpdateModesText()
    {
        string firstText;
        string secondText;

        if (firstHalf)
            firstText = "TC";
        else
            firstText = "RDR";

        if (secondHalf)
            secondText = "Central";
        else
            secondText = "Autonomous";


        modesText.text = firstText + " | " + secondText;

        if (error)
        {
            modesText.text += toSay;
            modesText.color = Color.red;
        }

    }

    private void DisplayPinkDot()
    {
        if (cameraController.IsTracking())
        {
            if (pinkDot.gameObject.activeInHierarchy)
                pinkDot.gameObject.SetActive(false);
            else
                pinkDot.gameObject.SetActive(true);
        }
        else if (!cameraController.IsTracking())
        {
            pinkDot.gameObject.SetActive(false);
        }
    }

    private void DisplayTrackingDots()
    {
        if (cameraController.IsTracking())
        {
            TargetUi targetUi = trackingDots.GetComponent<TargetUi>();
            if (cameraController.GetTrackedGameObject()?.transform.Find("Pivot"))
            {
                targetUi.SetTarget(cameraController.GetTrackedGameObject().transform.Find("Pivot"));
                targetUi.SetPosition(cameraController.GetTrackedGameObject().transform.Find("Pivot"));
            }
            else
            {
                targetUi.SetTarget(cameraController.GetTrackedGameObject().transform);
                targetUi.SetPosition(cameraController.GetTrackedGameObject().transform);
            }
            trackingDots.gameObject.SetActive(true);
        }
        else if (cameraController.isForcedTracking())
        {
            TargetUi targetUi = trackingDots.GetComponent<TargetUi>();
            targetUi.SetTarget(null);
            trackingDots.gameObject.SetActive(true);
        }
        else
        {
            trackingDots.gameObject.SetActive(false);
        }
    }

    public void SetTrackingDots(bool set)
    {
        cameraController.SetIsTracking(set);
        trackingDots.gameObject.SetActive(set);
    }

    private void DisplayBorder()
    {
        if (cameraController.GetCameraZoomLevel() < CameraController.CameraZoom.x24)
        {
            if (cameraController.GetCameraZoomLevel() > CameraController.CameraZoom.x1)
                Border.transform.localScale = new Vector3(0.65f, 0.8f, 1f);
            else
                Border.transform.localScale = Vector3.one;
            Border.gameObject.SetActive(true);
        }
        else
            Border.gameObject.SetActive(false);
    }

    public void UpdateZoomText()
    {
        string text;
        switch(cameraController.GetCameraZoomLevel())
        {
            default:
                text = "W";
                break;
            case CameraController.CameraZoom.x4:
                text = "M";
                break;
            case CameraController.CameraZoom.x24:
                text = "N";
                break;
        }
        zoomText.text = text;
    }
    
    public void SetNorth()
    {
        if (northText)
        {
            if (!trueNorth)
                northText.text = "True North";
            else
                northText.text = "Cabin North";
        }
        trueNorth = !trueNorth;
    }
    public bool GetRDRMode()
    {
        return !firstHalf;
    }
    public CameraController GetCameraController()
    {
        return cameraController;
    }
}
