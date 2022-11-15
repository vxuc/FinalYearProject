using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CursorControllerV2 : MonoBehaviour
{
    private CursorControls controls;

    //Minimap RT
    [SerializeField] Camera uiCamera;

    //Spawn Object
    public GameObject prefab;
    

    
    //LINE RENDERER
    //DOTS
    [Header("Dots")]
    [SerializeField] Transform dotParent;

    //LINES
    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] Transform lineParent;

    public LineController currentLine;
    [Header("Waypoint")]
    public Transform PlanePathParent;
    GameObject PlanePath;
    //Plane Height Slider
    [Header("Slider")]
    public Slider PlaneHeightSlider;
    public TextMeshProUGUI sliderValue;
    //Plane Speed Slider
    public Slider PlaneSpeedSlider;
    public TextMeshProUGUI sliderSpeedValue;

    //Detail Page
    [Header("Detail Page")]
    [SerializeField] GameObject detailPagePrefab;
    [SerializeField]  GameObject statsCanvas;

    public List<GameObject> prevWaypoints = new List<GameObject>();

    //Debugging
    [Header("DEBUGGING")]
    public TextMeshProUGUI text;
    private void Awake()
    {
        PlaneHeightSlider.onValueChanged.AddListener((v) =>
        {
            sliderValue.text = (v / 30.48).ToString("0" + "ft");
        });

        PlaneSpeedSlider.onValueChanged.AddListener((v) =>
        {
            sliderSpeedValue.text = v.ToString("0");
        });

        controls = new CursorControls();
    }

    void Start()
    {
        //Calls out Click Function
        controls.Mouse.Click.started += _ => StartedClick();
  
    }

    void Update()
    {
    
        //Right Click on Map
        if (Input.GetMouseButtonDown(1))
        {
            DetectObject();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoButton();
        }

    }



    //Code to insert waypoints and spawn in map
    private void DetectObject()
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        pointerData.position = controls.Mouse.Position.ReadValue<Vector2>();
        GetComponent<GraphicRaycaster>().Raycast(pointerData, results);


        if (Input.GetMouseButtonDown(0))
        {
            if (PlaneManager.Instance.spawnCount < 16 && PlaneManager.Instance.planeVal > 0)
            {
                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                    Debug.Log("Mouse Pos: " + pointerData.position);
                    if (currentLine == null)
                        PlanePath = Instantiate(PlanePathParent.gameObject, Vector3.zero, Quaternion.identity);

                    //The map position needs to be at the bottom left (0, 0) for this size to work unless RAYCAST [Later on during improvisation and debugging]
                    Vector3 pos = new Vector3(((pointerData.position.x - transform.position.x + 375) - GetComponent<RectTransform>().rect.width * 0.5f) / GetComponent<RectTransform>().rect.width * 1350000,
                        PlaneHeightSlider.value,
                        ((pointerData.position.y - transform.position.y + 375) - GetComponent<RectTransform>().rect.height * 0.5f) / GetComponent<RectTransform>().rect.height * 1350000);
                    Debug.Log("World Pos: " + pos);
                    //Instantiate(prefab, pos, Quaternion.identity);


                    //Draw a line
                    renderLine();
                    //Instantiates the Waypoints
                    var Point = Instantiate(prefab, pos, Quaternion.identity, PlanePath.transform);
                    prevWaypoints.Add(Point);
                    currentLine.AddPoint(Point.transform);
                }
            }
        }

        //Right Click on Map
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click is pressed");

            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);

                //The map position needs to be at the bottom left (0, 0) for this size to work unless RAYCAST [Later on during improvisation and debugging]
                Vector3 pos = new Vector3(((pointerData.position.x - transform.position.x + 375) - GetComponent<RectTransform>().rect.width * 0.5f) / GetComponent<RectTransform>().rect.width * 1350000,
                    9147000,
                    ((pointerData.position.y - transform.position.y + 375) - GetComponent<RectTransform>().rect.height * 0.5f) / GetComponent<RectTransform>().rect.height * 1350000);

                //Collider[] hitColliders = Physics.OverlapSphere(pos, 30);
                RaycastHit[] hitColliders = Physics.RaycastAll(pos, Vector3.down,float.MaxValue);
                foreach (var hitCollider in hitColliders)
                {
                    //Tag is on the plane icon as icon is on top of the plane
                    if(hitCollider.transform.gameObject.CompareTag("Planes"))
                    {
                        Debug.Log("Hit " + hitCollider.transform.gameObject.tag);
                        var statInfo = Instantiate(detailPagePrefab, new Vector3(1571, 407, 1), Quaternion.identity);
                        statInfo.GetComponent<DetailsPage>().SetPlane(hitCollider.transform.gameObject);
                        statInfo.transform.SetParent(statsCanvas.transform);
                    }
                }

            }


        }
    }

    public float GetHeightSliderValue()
    {
        return PlaneHeightSlider.value;
    }


    public LineController renderLine(bool customTransform = false, Transform t = null)
    {
        if (currentLine == null)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, customTransform ? t : PlanePath.transform).GetComponent<LineController>();
            return currentLine;
        }
        return null;
    }

    //On Press
    public void StartedClick()
    {
        Debug.Log("Start Click");
        DetectObject();
    }

    public void UndoButton()
    {
        GameObject prevWaypoint = prevWaypoints[prevWaypoints.Count - 1];
        currentLine.RemovePoint(prevWaypoint.transform);
        prevWaypoints.Remove(prevWaypoint);
        Destroy(prevWaypoint);
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

    public Transform GetPlanePathParent()
    {
        return PlanePath.transform;
    }

}
