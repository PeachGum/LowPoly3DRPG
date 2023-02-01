using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int index;

    public Item item;
    [HideInInspector]
    public Image icon;
    private Vector2 iconPosition;


    public SlotToolTip sizeToolTip;
    public InventoryUI inventoryUI;

    [HideInInspector]
    public Color transparentColor, iconColor;




    void Awake()
    {
        icon = transform.GetChild(0).GetComponentInChildren<Image>();
        iconPosition = icon.rectTransform.anchoredPosition;

        transparentColor = new Color(255f, 255f, 255f, 0f);
        iconColor = new Color(255f, 255f, 255f, 255f);
        //icon.color = transparentColor;
        icon.enabled = false;
        sizeToolTip = GameObject.Find("UIManager_Scripts").GetComponent<SlotToolTip>();
    }
    public void AddItem(Item newItem)
    {
        if (newItem != null)
        {
            //Inventory.instance.items[index] = newItem;
            item = newItem;
            icon.enabled = true;
            icon.color = iconColor;
            icon.sprite = newItem.itemImage;
            
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                item.Buy();
                sizeToolTip.HideToolTip();
            }
        }
    }

    //���콺 �����Ͱ� ���� ���� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null && sizeToolTip != null)
        {
            sizeToolTip.ShowToolTip(item, transform.position, true);
            
        }
    }

    //���콺 �����Ͱ� ���Կ��� ����� ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null && sizeToolTip != null)
        {
            sizeToolTip.HideToolTip();
        }
    }

    //���콺�� �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null && sizeToolTip != null)
        {
            
            DragSlot.instance.item = item;
            DragSlot.instance.quantity = 1;
            DragSlot.instance.index = index;
            DragSlot.instance.dragBeginType = DragSlot.InventoryType.Shop;
            //inventoryUI.inventoryParent.transform.SetAsLastSibling();
            //transform.SetAsLastSibling();
            sizeToolTip.transform.SetAsLastSibling();
            sizeToolTip.canTooltip = false;

            inventoryUI.dragSlotImage.BeginDrag(item);

            icon.raycastTarget = false;

        }
        
    }

    //���콺�� �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {

    }

    //���콺�� �巡�� ����
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.dragEndType = DragSlot.InventoryType.Null;
        DragSlot.instance.item = null;
        DragSlot.instance.quantity = -1;
        DragSlot.instance.index = -1;
        DragSlot.instance.dragBeginType = DragSlot.InventoryType.Null;

        sizeToolTip.canTooltip = true;

        inventoryUI.dragSlotImage.EndDrag();

        icon.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Normal)
        {
            DragSlot.instance.dragEndType = DragSlot.InventoryType.Shop;
            DragSlot.instance.item.Sell(DragSlot.instance.index);
        }

        if(DragSlot.instance.dragBeginType == DragSlot.InventoryType.Shop)
        {
            return;
        }
    }
}