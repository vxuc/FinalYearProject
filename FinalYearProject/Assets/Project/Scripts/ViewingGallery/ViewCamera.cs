using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCamera : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform target;
    [SerializeField] float distanceToTarget = 10;
    [SerializeField] [Range(0, 360)] int maxRotationInOneSwipe = 180;
    [SerializeField] [Range(0, 100)] float zoomSpeed = 10;

    Vector3 previousPosition;
    Vector3 resetPosition;
    Quaternion resetRotation;
    [SerializeField] Vector3 offset;

    private void Start()
    {
        resetPosition = transform.position;
        resetRotation = transform.rotation;
    }

    void Update()
    {
        if (target == null)
        {
            if (AircraftInfoManager.Instance.currentAircraft != null)
                target = AircraftInfoManager.Instance.currentAircraft.transform;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
            ResetTransform();

        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * maxRotationInOneSwipe; // camera moves horizontally
            float rotationAroundXAxis = direction.y * maxRotationInOneSwipe; // camera moves vertically

            cam.transform.position = target.position + offset;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <� This is what makes it work!

            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            previousPosition = newPosition;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cam.fieldOfView += zoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cam.fieldOfView -= zoomSpeed;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 5f, 100f);
    }

    void ResetTransform()
    {
        transform.position = resetPosition;
        transform.rotation = resetRotation;
        cam.fieldOfView = 60;
    }
}