using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    LineController lc;
    Rigidbody rb;
    Vector3 desiredPosition;
    int index = 0;
    [SerializeField] float movementSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lc = FindObjectOfType<LineController>();
        desiredPosition = lc.points[0].position;
        transform.position = desiredPosition;
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, desiredPosition) < 5)
        {
            index++;
            if (index >= lc.points.Count) index = 0;
            UpdateDesiredPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.fixedDeltaTime * movementSpeed);
        transform.rotation = Quaternion.LookRotation(transform.position - desiredPosition, Vector3.up);
        //rb.MovePosition(desiredPosition);
    }

    void UpdateDesiredPosition()
    {
        desiredPosition = lc.points[index].position;
    }
}
