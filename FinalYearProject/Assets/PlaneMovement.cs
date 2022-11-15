using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 desiredPosition = Vector3.zero;
    int index = 0;
    public float movementSpeed;
    public float tempSpeed;

    public List<Transform> destinations;
    [SerializeField] bool flip = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (destinations.Count > 0)
            desiredPosition = destinations[0].position;
        transform.position = desiredPosition;
        tempSpeed = movementSpeed;
    }


    private void FixedUpdate()
    {
        if (destinations.Count == 0)
            return;

        if(Vector3.Distance(transform.position, desiredPosition) < 5000)
        {
            index++;
            if (index >= destinations.Count) index = 0;
            UpdateDesiredPosition();
        }
        else if(Vector3.Distance(transform.position, desiredPosition) < 10000)
            movementSpeed = tempSpeed * 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.fixedDeltaTime * movementSpeed);

        var newRotation = Quaternion.LookRotation(transform.position - desiredPosition, Vector3.up).eulerAngles;
        if (flip)
            newRotation += new Vector3(0,180,0);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.fixedDeltaTime);
        Debug.Log("SPEED: " + movementSpeed);
    }

    void UpdateDesiredPosition()
    {
        desiredPosition = destinations[index].position;

        int ind = index - 2;
        if (ind < 0) ind = destinations.Count - 1;
        Debug.Log(IsLeft(transform.position - destinations[ind].position, desiredPosition - transform.position));

        //float angle = IsLeft(transform.position - destinations[ind].position, desiredPosition - transform.position) ? -45 : 45;
        //Vector3 newDir = Quaternion.Euler(0, 0f, 45f) * transform.forward;
        //Vector3 newRotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newDir.z);
        //StartCoroutine(Bank(newRotation));
        //transform.Rotate(transform.right, isLeft ? 25f : -25f;

        movementSpeed = tempSpeed * 0.5f;
        transform.Rotate(Vector3.forward, IsLeft(transform.position - destinations[ind].position, desiredPosition - transform.position) ? -35f : 35f);
        Invoke(nameof(ResetSpeed), 2f);
    }

    private void ResetSpeed()
    {
        movementSpeed = tempSpeed;
    }

    IEnumerator Bank(Vector3 newRotation)
    {
        while (Vector3.Distance(transform.rotation.eulerAngles, Quaternion.Euler(newRotation).eulerAngles) > 1)
        {
            movementSpeed = tempSpeed * 0.25f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.fixedDeltaTime * 0.5f);
        }
        movementSpeed = tempSpeed;
        yield return null;
    }

    bool IsLeft(Vector3 A, Vector3 B)
    {
        return -A.x * B.z + A.z * B.x < 0;
    }
}
