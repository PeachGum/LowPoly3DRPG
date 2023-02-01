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
        Debug.Log($"{index}��° ������ ClearSlot");
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
                //�� ���Կ��� �ǸŴ� ������� ����
                return;
            }
            inventoryUI.sizeToolTip.HideToolTip();
        }
    }

    //���콺 �����Ͱ� ���� ���� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && inventoryUI.sizeToolTip != null)
        {
            inventoryUI.sizeToolTip.ShowToolTip(item, transform.position, false);
        }
    }

    //���콺 �����Ͱ� ���Կ��� ����� ��
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
            //�κ��丮 â�� �巡�� ���� ���
            if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Normal)
            {
                quantity = DragSlot.instance.quantity;
                AddItem(DragSlot.instance.item);

                Inventory.instance.quickReceive[index] = DragSlot.instance.index;
            }

            //�� 
            else if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Quick)
            {
                //��ӵ� ���Կ� �������� ���� ��� �巡�� ������ �����Կ� ������ ����
                if (item != null)
                {
                    int tempindex = -1;

                    Inventory.instance.quickSlots[DragSlot.instance.index].quantity = quantity;
                    Inventory.instance.quickSlots[DragSlot.instance.index].AddItem(item);


                    //��ӵ� ����
                    quantity = DragSlot.instance.quantity;
                    AddItem(DragSlot.instance.item);

                    tempindex = Inventory.instance.quickReceive[DragSlot.instance.index];
                    Inventory.instance.quickReceive[DragSlot.instance.index] = Inventory.instance.quickReceive[index];
                    Inventory.instance.quickReceive[index] = tempindex;

                }
                else
                //��ӵ� ���Կ� �������� ���� ���
                {
                    quantity = DragSlot.instance.quantity;
                    AddItem(DragSlot.instance.item);
                    
                    Inventory.instance.quickReceive[index] = Inventory.instance.quickReceive[DragSlot.instance.index];

                    Inventory.instance.quickSlots[DragSlot.instance.index].ClearSlot();
                }
                
            }
            //���â�������� �巡��������
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
