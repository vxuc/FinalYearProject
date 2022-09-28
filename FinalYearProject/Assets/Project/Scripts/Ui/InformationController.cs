using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationController : MonoBehaviour
{
    public Transform userCamera;
    CameraController cameraController;

    [Header("Ui")]
    public TextMeshProUGUI rotationText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI modesText;
    public Image trackingDots;
    public Image pinkDot;

    [Header("ModesText")]
    bool firstHalf = true; //TC/RDR
    bool secondHalf = true; //Central/Autonomous

    bool error = false;
    string toSay = "Error";
    float errorTimer = 3;


    void Start()
    {
        modesText.text = "TC | Central";

        if (userCamera.GetComponent<CameraController>())
            cameraController = userCamera.GetComponent<CameraController>();
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

    }

    private void updateTimeText()
    {
        string time = System.DateTime.Now.ToString("HH:mm:ss");
        string date = System.DateTime.Now.ToString("dd/MM/yy");
        timeText.text = date + "    " + time;
    }

    private void updateRotationText()
    {
        float az;
        if (userCamera.rotation.eulerAngles.y > 359)
            az = 0;
        else
            az = userCamera.rotation.eulerAngles.y;
        //string az = (userCamera.rotation.y * Mathf.Rad2Deg).ToString("F0");
        //string el = userCamera.rotation.eulerAngles.x.ToString("F1");
        float angle = userCamera.rotation.eulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;
        float el = -angle;
        if(el > 0)
            rotationText.text = "TL :\nAZ : +" + az.ToString("F0") + "�\nEL : +" + el.ToString("F1") + "�";
        else
            rotationText.text = "TL :\nAZ : +" + az.ToString("F0") + "�\nEL : " + el.ToString("F1") + "�";
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
        if(cameraController.isTracking())
        {
            if (pinkDot.gameObject.activeInHierarchy)
                pinkDot.gameObject.SetActive(false);
            else
                pinkDot.gameObject.SetActive(true);
        }
        else if(!cameraController.isTracking())
        {
            pinkDot.gameObject.SetActive(false);
        }
    }

    private void DisplayTrackingDots()
    {
        if (cameraController.isTracking())
        {
            if (cameraController.GetTrackedGameObject().transform.Find("Pivot"))
            {
                TargetUi targetUi = trackingDots.GetComponent<TargetUi>();
                targetUi.SetTarget(cameraController.GetTrackedGameObject().transform.Find("Pivot"));
                targetUi.SetPosition(cameraController.GetTrackedGameObject().transform.Find("Pivot"));
            }
            trackingDots.gameObject.SetActive(true);
        }
        else
            trackingDots.gameObject.SetActive(false);
    }

    public bool GetRDRMode()
    {
        return !firstHalf;
    }
}
