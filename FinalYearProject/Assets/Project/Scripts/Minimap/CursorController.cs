using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    private CursorControls controls;

    //Minimap RT
    [SerializeField] Camera uiCamera;

    //Spawn Object
    [SerializeField] GameObject prefab;


    //LINE RENDERER
    //DOTS
    [Header("Dots")]
    [SerializeField] Transform dotParent;
    [SerializeField] private GameObject dotPrefab;

    //LINES
    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] Transform lineParent;

    public LineController currentLine;




    private void Awake()
    {
        controls = new CursorControls();
    }

    void Start()
    {
        //Calls out Click Function
        controls.Mouse.Click.started += _ => StartedClick();
  
    }

    void Update()
    {
        ////Enables cursor only when UI is open
        //if (!UIControls.openMenu)
        //{
        //    Cursor.visible = false;
        //    OnDisable();
        //}
        //else if(UIControls.openMenu)
        //{
        //    Cursor.visible = true;
        //    OnEnable();
        //}      

    }


    //Code to insert waypoints and spawn in map
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

            //The map position needs to be at the bottom left (0, 0) for this size to work unless RAYCAST [Later on during improvisation and debugging]
            Vector3 pos = new Vector3((pointerData.position.x - 750 * 0.5f) / 750 * 1350000, 50000, (pointerData.position.y - 750 * 0.5f) / 750 * 1350000);
            Debug.Log("World Pos: " + pos);
            Instantiate(prefab, pos, Quaternion.identity);

            //Draw a line
            renderLine();

            currentLine.AddPoint(Instantiate(prefab, pos, Quaternion.identity).transform);
        }

    }

    void renderLine()
    {
        if(currentLine == null)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();
        }
    }    

    //On Press
    public void StartedClick()
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
