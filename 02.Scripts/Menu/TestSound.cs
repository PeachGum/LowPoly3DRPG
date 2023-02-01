using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestSound : MonoBehaviour, IEndDragHandler
{

    public void OnEndDrag(PointerEventData eventData)
    {
        AudioManager.instance.SFXPlay("ItemPickUp");
    }
}
