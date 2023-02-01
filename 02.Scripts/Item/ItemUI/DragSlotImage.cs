using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlotImage : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if(image.enabled)
        {
            transform.position = Input.mousePosition;
        }
    }
    public void BeginDrag(Item item)
    {
        image.enabled = true;
        image.sprite = item.itemImage;
    }

    public void EndDrag()
    {
        image.enabled = false;
        image.sprite = null;
    }


}
