using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUi : MonoBehaviour
{
    Transform target;

    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(target.position);
        transform.position = new Vector3(pos.x, pos.y, 1f);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
