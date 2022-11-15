using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlaneMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 desiredPosition = Vector3.zero;
    int index = 0;
    public float movementSpeed;

    public List<Transform> destinations;
    [SerializeField] bool flip = false;

    public CursorControllerV2 cursorControllerV2;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (destinations.Count > 0)
            desiredPosition = destinations[0].position;
        transform.position = desiredPosition;

        movementSpeed = cursorControllerV2.PlaneSpeedSlider.value;
    }


    private void FixedUpdate()
    {
        if (destinations.Count == 0)
            return;

        if(Vector3.Distance(transform.position, desiredPosition) < 5)
        {
            index++;
            if (index >= destinations.Count) index = 0;
            UpdateDesiredPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.fixedDeltaTime * movementSpeed);

        var newRotation = Quaternion.LookRotation(transform.position - desiredPosition, Vector3.up).eulerAngles;
        if (flip)
            newRotation += new Vector3(0,180,0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.fixedDeltaTime);


        Debug.Log("Plane Mov Speed: " + movementSpeed);
    }

    void UpdateDesiredPosition()
    {
        desiredPosition = destinations[index].position;
    }

    
}
