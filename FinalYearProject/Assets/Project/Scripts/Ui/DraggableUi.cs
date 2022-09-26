using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUi : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform dragRectTransform;
    public bool goToFront = true;
    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(goToFront)
        dragRectTransform.SetAsLastSibling();
    }
}
