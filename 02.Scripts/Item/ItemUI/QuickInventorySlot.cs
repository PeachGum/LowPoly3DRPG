using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuickInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    public Item item;
    [HideInInspector]
    public Image icon;

    public InventoryUI inventoryUI;
    public int index, quantity = 0;

    [HideInInspector]
    public TextMeshProUGUI quantityText;

    private Color transparentColor, iconColor;

    void Start()
    {
        icon = transform.GetChild(0).GetComponentInChildren<Image>();
        icon.enabled = false;

        quantityText = GetComponentInChildren<TextMeshProUGUI>();
        transparentColor = new Color(255f, 255f, 255f, 0f);
        iconColor = new Color(255f, 255f, 255f, 255f);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        UpdateSlot();
    }


    public void UpdateQuantity(int inventoryQuantity)
    {
        quantityText.enabled = true;
        quantity = inventoryQuantity;

        if (quantity <= 0)
        {
            ClearSlot();
            return;
        }
        quantityText.text = quantity.ToString();

    }
    public void UpdateSlot()
    {
        if (item != null)
        {
            if (icon != null)
            {
                icon.enabled = true;
                icon.color = iconColor;
                icon.sprite = item.itemImage;
            }

            quantityText.enabled = true;

            if (item.itemType == Item.ItemType.Equipment)
            {
                quantityText.text = "";
            }
            else
            {
                quantityText.text = quantity.ToString();
            }
        }
    }
    public void UseItem()
    {
        if (item != null && Inventory.instance.quickReceive[index] != -1)
        {
            item.Use(Inventory.instance.quickReceive[index]);
        }
    }

    public void ClearSlot()
    {
        Debug.Log($"{index}번째 퀵슬롯 ClearSlot");
        quantity = 0;
        item = null;

        icon.sprite = null;
        icon.color = transparentColor;


        quantityText.text = "";
        quantityText.enabled = false;

        Inventory.instance.quickReceive[index] = -1;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                
                UseItem();
            }

            else if (item != null && inventoryUI.dialogueUIManager.listUI[5].gameObject.activeSelf)
            {
                //퀵 슬롯에서 판매는 허용하지 않음
                return;
            }
            inventoryUI.sizeToolTip.HideToolTip();
        }
    }

    //마우스 포인터가 슬롯 위에 있을때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && inventoryUI.sizeToolTip != null)
        {
            inventoryUI.sizeToolTip.ShowToolTip(item, transform.position, false);
        }
    }

    //마우스 포인터가 슬롯에서 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null && inventoryUI.sizeToolTip != null)
        {
            inventoryUI.sizeToolTip.HideToolTip();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.BeginDrag(item, index, quantity, DragSlot.InventoryType.Quick);
            inventoryUI.dragSlotImage.BeginDrag(item);
            inventoryUI.sizeToolTip.canTooltip = false;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragSlot.instance.dragEndType == DragSlot.InventoryType.Null || DragSlot.instance.dragEndType == DragSlot.InventoryType.Normal || item == null)
        {
            ClearSlot();
        }
        DragSlot.instance.EndDrag();
        inventoryUI.dragSlotImage.EndDrag();
        inventoryUI.sizeToolTip.canTooltip = true;
        //icon.raycastTarget = true;
    }

    
    public void OnDrop(PointerEventData eventData)
    {

        DragSlot.instance.dragEndType = DragSlot.InventoryType.Quick;
        if(DragSlot.instance.dragBeginType != DragSlot.InventoryType.Null)
        {
            //인벤토리 창에 드래그 했을 경우
            if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Normal)
            {
                quantity = DragSlot.instance.quantity;
                AddItem(DragSlot.instance.item);

                Inventory.instance.quickReceive[index] = DragSlot.instance.index;
            }

            //퀵 
            else if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Quick)
            {
                //드롭된 슬롯에 아이템이 있을 경우 드래그 시작점 퀵슬롯에 아이템 삽입
                if (item != null)
                {
                    int tempindex = -1;

                    Inventory.instance.quickSlots[DragSlot.instance.index].quantity = quantity;
                    Inventory.instance.quickSlots[DragSlot.instance.index].AddItem(item);


                    //드롭된 슬롯
                    quantity = DragSlot.instance.quantity;
                    AddItem(DragSlot.instance.item);

                    tempindex = Inventory.instance.quickReceive[DragSlot.instance.index];
                    Inventory.instance.quickReceive[DragSlot.instance.index] = Inventory.instance.quickReceive[index];
                    Inventory.instance.quickReceive[index] = tempindex;

                }
                else
                //드롭된 슬롯에 아이템이 없을 경우
                {
                    quantity = DragSlot.instance.quantity;
                    AddItem(DragSlot.instance.item);
                    
                    Inventory.instance.quickReceive[index] = Inventory.instance.quickReceive[DragSlot.instance.index];

                    Inventory.instance.quickSlots[DragSlot.instance.index].ClearSlot();
                }
                
            }
            //장비창에서부터 드래그했을때
            else if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Equipment)
            {
                quantity = DragSlot.instance.quantity;
                AddItem(DragSlot.instance.item);

                Inventory.instance.quickReceive[index] = DragSlot.instance.index;
                Inventory.instance.quickReceiveIsEquipping[index] = true;
            }
        }
        else
        {
            ClearSlot();
        }


    }
}
