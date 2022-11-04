using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 desiredPosition = Vector3.zero;
    int index = 0;
    public float movementSpeed;

    public List<Transform> destinations;
    [SerializeField] bool flip = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (destinations.Count > 0)
            desiredPosition = destinations[0].position;
        transform.position = desiredPosition;
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
    }

    void UpdateDesiredPosition()
    {
        desiredPosition = destinations[index].position;
    }
}
