using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatController : MonoBehaviour
{
    [SerializeField] float heatValue;

    public float GetHeatValue()
    {
        return heatValue;
    }

    public void SetHeatValue(float heatValue)
    {
        this.heatValue = heatValue;
    }
}
