    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Image image;

    void Start()
    {
        image = GetComponentInParent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Transform pa = transform.parent;
        pa.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
    }
}
