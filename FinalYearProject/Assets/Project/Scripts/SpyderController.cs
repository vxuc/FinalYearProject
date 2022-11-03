using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpyderController : MonoBehaviour
{
    [Header("SpyderDirection")]
    public TextMeshProUGUI spyderDirText;
    public TMP_InputField spyderDirInputField;
    public Slider spyderDirSlider;
    public CameraController SpyderMainCamera;
    bool valueChange;

    // Update is called once per frame
    void Update()
    {
        SpyderDirUpdate();
    }

    void SpyderDirUpdate()
    {
        if (spyderDirSlider)
        {
            spyderDirSlider.onValueChanged.AddListener //If Slider Changes
            (delegate
            {
                valueChange = true;
            }
            );
        }
        if (spyderDirInputField)
        {
            if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrWhiteSpace(spyderDirInputField.text)) //If input field ain't empty and enter is pressed
            {
                spyderDirSlider.value = int.Parse(spyderDirInputField.text); //change slider value
                spyderDirInputField.text = null; //reset
            }
        }
        if (valueChange) //update when there is a change 
        {
            spyderDirText.text = "Current Spyder Direction : " + spyderDirSlider.value.ToString() + "°";
            this.transform.rotation = Quaternion.Euler(new Vector3 (0,spyderDirSlider.value,0));
            SpyderMainCamera.SpyderDirChange(); //Reset on where the camera is facing
            valueChange = false;
        }
    }
}
