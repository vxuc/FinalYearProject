using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUi : MonoBehaviour, IDragHandler
{
    [SerializeField] private RectTransform dragRectTransform;
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        dragRectTransform.anchoredPosition += eventData.delta;
    }
}
