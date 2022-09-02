using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    private CursorControls controls;
    [SerializeField] Camera uiCamera;
    [SerializeField] GameObject prefab;

    private void Awake()
    {
        controls = new CursorControls();
        //uiCamera = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>();
    }

    void Start()
    {
        //Calls out Click Function
        controls.Mouse.Click.started += _ => StartedClick();
  
    }

    void Update()
    {
        //Enables cursor only when UI is open
        if (!UIControls.openMenu)
        {
            Cursor.visible = false;
            OnDisable();
        }
        else if(UIControls.openMenu)
        {
            Cursor.visible = true;
            OnEnable();
        }      

    }

    private void DetectObject()
    {
       
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        pointerData.position = controls.Mouse.Position.ReadValue<Vector2>();
        GameObject.Find("MenuUICanvas").GetComponent<GraphicRaycaster>().Raycast(pointerData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit " + result.gameObject.name);
            Debug.Log("Mouse Pos: " + pointerData.position);

            Vector3 pos = new Vector3((pointerData.position.x - 750 * 0.5f) / 750 * 20000f, 10000, (pointerData.position.y - 750 * 0.5f) / 750 * 20000f);
            Debug.Log("World Pos: " + pos);
            Instantiate(prefab, pos, Quaternion.identity);
        }

    }

    //On Press
    void StartedClick()
    {
        Debug.Log("Start Click");
        DetectObject();
    }

    //Enable & Disable Controls
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
