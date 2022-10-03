using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 desiredPosition;
    int index = 0;
    public float movementSpeed;

    public List<Transform> destinations;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        desiredPosition = destinations[0].position;
        transform.position = desiredPosition;
    }


    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, desiredPosition) < 5)
        {
            index++;
            if (index >= destinations.Count) index = 0;
            UpdateDesiredPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.fixedDeltaTime * movementSpeed);
        transform.rotation = Quaternion.LookRotation(transform.position - desiredPosition, Vector3.up);
        
    }

    void UpdateDesiredPosition()
    {
        desiredPosition = destinations[index].position;
    }
}
