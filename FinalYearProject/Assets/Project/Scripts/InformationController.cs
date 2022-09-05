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
    public Image pinkDot;

    [Header("ModesText")]
    bool firstHalf = true; //TC/RDR
    bool secondHalf = true; //Central/Autonomous


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
            if (Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.Joystick1Button8))
            {
                UpdateModes();
            }
        }

        if (pinkDot && cameraController)
        {
            DisplayPinkDot();
        }
    }

    private void updateTimeText()
    {
        string time = System.DateTime.Now.ToString("hh:mm:ss");
        string date = System.DateTime.Now.ToString("dd//MM/yy");
        timeText.text = date + "    " + time;
    }

    private void updateRotationText()
    {
        float az = userCamera.rotation.eulerAngles.y;
        //string az = (userCamera.rotation.y * Mathf.Rad2Deg).ToString("F0");
        //string el = userCamera.rotation.eulerAngles.x.ToString("F1");
        float angle = userCamera.rotation.eulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;
        float el = -angle;
        if(el > 0)
            rotationText.text = "TL :\nAZ : +" + az.ToString("F0") + "°\nEL : +" + el.ToString("F1") + "°";
        else
            rotationText.text = "TL :\nAZ : +" + az.ToString("F0") + "°\nEL : " + el.ToString("F1") + "°";
    }

    private void UpdateModes()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            if (firstHalf)
                firstHalf = false;
            else
                firstHalf = true;
        }

        if(Input.GetKeyDown(KeyCode.Joystick1Button8))
        {
            if (secondHalf)
                secondHalf = false;
            else
                secondHalf = true;
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

    }

    private void DisplayPinkDot()
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button1) && cameraController.isTracking())
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

}
