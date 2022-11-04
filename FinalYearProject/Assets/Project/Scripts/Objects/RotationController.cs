using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public bool fX, fY, fZ;
    [SerializeField] float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        if(fX)
        {
            gameObject.transform.Rotate(rotationSpeed,0,0);
        }
        if (fY)
        {
            gameObject.transform.Rotate(0, rotationSpeed, 0);
        }
        if (fZ)
        {
            gameObject.transform.Rotate(0, 0, rotationSpeed);
        }
    }
}
