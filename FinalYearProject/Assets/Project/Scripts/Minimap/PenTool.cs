using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    [Header("Dots")]
    [SerializeField] Transform dotParent;
    [SerializeField] private GameObject dotPrefab;

    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] Transform lineParent;

    private LineController currentLine;


   
}
