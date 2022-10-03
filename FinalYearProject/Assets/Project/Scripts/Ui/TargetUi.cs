using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUi : MonoBehaviour
{
    Transform target;
    Vector3 pos;

    void Update()
    {
        pos = Camera.main.WorldToScreenPoint(target.position);
        transform.position = new Vector3(pos.x, pos.y, 1f);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetPosition(Transform target)
    {
        transform.position = new Vector3(target.position.x, target.position.y, 1f);
    }
}
