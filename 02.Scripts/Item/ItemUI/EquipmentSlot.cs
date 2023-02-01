using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //0 : �Ӹ� 1: ���� 2: ���� 3: ���� 4: ���� 5: �Ź�
    private Sprite baseSprite;
    [HideInInspector]
    public EquipmentItem equipmentItem;
    
    private Image icon;
    
    private SlotToolTip sizeToolTip;
    private InventoryUI inventoryUI;
    private Vector2 iconPosition;

    public int index;

    void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        baseSprite = icon.sprite;
        inventoryUI = GameObject.Find("UIManager_Scripts").GetComponent<InventoryUI>();
        sizeToolTip = GameObject.Find("UIManager_Scripts").GetComponent<SlotToolTip>();
        iconPosition = icon.rectTransform.anchoredPosition; ;

    }
    public void Add(EquipmentItem eq)
    {
        equipmentItem = eq;
        icon.sprite = eq.itemImage;
    }
    public void Remove()
    {
        if (equipmentItem != null)
        {
            equipmentItem = null;
            icon.sprite = baseSprite;
        }
        else
        {
            return;
        }
    }

    public void Remove(int inventoryIndex)
    {

        if (equipmentItem != null)
        {
            int equipIndex = (int)equipmentItem.equipType;
            Player_Equipment.instance.UnEquip(equipIndex, inventoryIndex);
            equipmentItem = null;
            icon.sprite = baseSprite;
        }
        else
        {
            return;
        }
    }

    #region MouseController
    public void OnPointerClick(PointerEventData eventData)
    {
        //��Ŭ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (equipmentItem != null)
            {
                equipmentItem.Use(index);
                sizeToolTip.HideToolTip();
            }
        }
    }

    //���콺 �����Ͱ� ���� ���� ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equipmentItem != null)
        {
            sizeToolTip.ShowToolTip(equipmentItem, transform.position, true);

        }
    }

    //���콺 �����Ͱ� ���Կ��� ����� ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (equipmentItem != null)
        {
            sizeToolTip.HideToolTip();
        }
    }

    //���콺�� �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (equipmentItem != null)
        {
            DragSlot.instance.BeginDrag(equipmentItem, index, 1, DragSlot.InventoryType.Equipment);
            inventoryUI.dragSlotImage.BeginDrag(equipmentItem);
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

        if (equipmentItem == null)
        {
            Remove();
        }

        sizeToolTip.canTooltip = true;
        icon.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragSlot.instance.dragEndType = DragSlot.InventoryType.Equipment;
        DragSlot.instance.item.Use(DragSlot.instance.index);
    }

    #endregion
}
