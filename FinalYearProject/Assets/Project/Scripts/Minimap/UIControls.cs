using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControls : MonoBehaviour
{
    public GameObject userInterface;
    private KeyboardControls controls;

    public static bool openMenu = false;

    private void Awake()
    {
        controls = new KeyboardControls();
    }

    private void Start()
    {
        controls.Keyboard.OpenMenu.started += _ => Open();
    }

    void Open()
    {
        if(!openMenu)
        {
            userInterface.SetActive(true);
            openMenu = true;
        }
        else if(openMenu)
        {
            userInterface.SetActive(false);
            openMenu = false;
        }
    }


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
