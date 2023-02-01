using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    public int index;

    [HideInInspector]
    public Image icon;

    private Vector2 iconPosition;
    [HideInInspector]
    public TextMeshProUGUI quantityText;
    public InventoryUI inventoryUI;

    [HideInInspector]
    public Color transparentColor;
    [HideInInspector]
    public Color iconColor;




    void Awake()
    {
        icon = transform.GetChild(0).GetComponentInChildren<Image>();
        quantityText = GetComponentInChildren<TextMeshProUGUI>();
        iconPosition = icon.rectTransform.anchoredPosition;

        transparentColor = new Color(255f, 255f, 255f, 0f);
        iconColor = new Color(255f, 255f, 255f, 255f);

        icon.enabled = false;
        icon.raycastTarget = false;

    }
    public void AddItem(Item newItem)
    {
        if (newItem != null)
        {
            icon.enabled = true;
            icon.color = iconColor;
            icon.sprite = Inventory.instance.items[index].itemImage;
            
        }
    }

    public void ClearSlot()
    {
        Inventory.instance.itemQuantity[index] = 0;
        Inventory.instance.items[index] = null;
        
        icon.sprite = null;
        icon.color = transparentColor;


        quantityText.text = "";
        quantityText.enabled = false;
    }
    public void UpdateQuantity(int inventoryQuantity)
    {
        quantityText.enabled = true;
        Inventory.instance.itemQuantity[index] = inventoryQuantity;

        if (Inventory.instance.itemQuantity[index] <= 0)
        {
            ClearSlot();
            Inventory.instance.Remove(Inventory.instance.items[index]);
            return;
        }

        quantityText.text = Inventory.instance.itemQuantity[index].ToString();
    }

    public void UpdateSlot()
    {
        if(Inventory.instance.items[index] == null)
        {
            icon.enabled = false;
            icon.color = transparentColor;
        }

        if(Inventory.instance.items[index] != null)
        {
            if(icon != null)
            {
                icon.enabled = true;
                icon.color = iconColor;
                icon.sprite = Inventory.instance.items[index].itemImage;
            }

            if (Inventory.instance.itemQuantity[index] > 0)
            {
                quantityText.enabled = true;
                if(Inventory.instance.items[index].itemType == Item.ItemType.Equipment)
                {
                    quantityText.text = "";
                }
                else 
                {
                    quantityText.text = Inventory.instance.itemQuantity[index].ToString();
                }
                
            }
        }
    }

    public void UseItem()
    {
        if (Inventory.instance.items[index] != null)
        {
            Inventory.instance.items[index].Use(index);
            inventoryUI.sizeToolTip.HideToolTip();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Inventory.instance.items[index] != null && !inventoryUI.dialogueUIManager.listUI[5].gameObject.activeSelf)
            {
                UseItem();
            }

            else if(Inventory.instance.items[index] != null && inventoryUI.dialogueUIManager.listUI[5].gameObject.activeSelf)
            {
                Inventory.instance.items[index].Sell(index);
            }
            inventoryUI.sizeToolTip.HideToolTip();
        }
    }

    //���콺 �����Ͱ� ���� ���� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Inventory.instance.items[index] != null && inventoryUI.sizeToolTip != null)
        {
            inventoryUI.sizeToolTip.ShowToolTip(Inventory.instance.items[index], transform.position, true);
            
        }
    }

    //���콺 �����Ͱ� ���Կ��� ����� ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Inventory.instance.items[index] != null && inventoryUI.sizeToolTip != null)
        {
            inventoryUI.sizeToolTip.HideToolTip();
        }
    }

    //���콺�� �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Inventory.instance.items[index] != null)
        {
            DragSlot.instance.BeginDrag(Inventory.instance.items[index], index, Inventory.instance.itemQuantity[index], DragSlot.InventoryType.Normal);
            inventoryUI.dragSlotImage.BeginDrag(Inventory.instance.items[index]);
            inventoryUI.sizeToolTip.canTooltip = false;
        }
        
    }

    //���콺�� �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    //���콺�� �巡�� ����
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.EndDrag();
        inventoryUI.dragSlotImage.EndDrag();

        if (Inventory.instance.items[index] == null)
        {
            ClearSlot();
        }
        
        inventoryUI.sizeToolTip.canTooltip = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragSlot.instance.dragEndType = DragSlot.InventoryType.Normal;
        MoveItem();
    }


    public void MoveItem()
    {
        //�Ϲ� �κ��丮���� �Ϲ� ��ĭ �κ��丮�� �̵���
        if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Normal && Inventory.instance.items[index] == null)
        {
            Inventory.instance.TradeInvenToDrag(null, index, 0);
        }
        //�Ϲ� �κ��丮���� �Ϲ� �κ��丮�� �̵���
        else if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Normal && Inventory.instance.items[index] != null)
        {
            Inventory.instance.TradeInvenToDrag(Inventory.instance.items[index], index, Inventory.instance.itemQuantity[index]);
        }

        //�� �κ��丮���� �Ȱ��� ������ �κ��丮�� �Ű��� ���
        else if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Quick && DragSlot.instance.item == Inventory.instance.items[index])
        {
            Inventory.instance.quickSlots[DragSlot.instance.index].ClearSlot();
        }

        //���â���� �Ϲ� ������ â
        else if (DragSlot.instance.dragBeginType == DragSlot.InventoryType.Equipment)
        {
            Player_Equipment.instance.UnEquip(DragSlot.instance.index, index);

        }
        //�������� �巡�׷� ����
        else if(DragSlot.instance.dragBeginType == DragSlot.InventoryType.Shop)
        {
            DragSlot.instance.item.Buy();
        }
        else
        {
            return;
        }
    }

    
}