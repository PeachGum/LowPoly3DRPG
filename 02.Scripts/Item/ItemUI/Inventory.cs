using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    
    #endregion

    public delegate void OnItemChanged(int index);
    public delegate void OnQuantityChanged(Item item, int index, int val);
    public delegate void OnQuickQuantityChanged(Item item, int index);
    public delegate void OnUpdateMoney();

    //원본 함수는 InventoryUI에 있음
    public OnItemChanged onItemChangedCallback;
    public OnQuantityChanged onQuantityChangedCallback;
    public OnQuickQuantityChanged onQuickQuantityChangedCallback;
    public OnUpdateMoney onOnUpdateMoneyCallback;

    public List<Item> items = new List<Item>(32);
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(32);
    public List<int> itemQuantity = new List<int>(32);
    public List<QuickInventorySlot> quickSlots = new List<QuickInventorySlot>(8);
    public List<int> quickReceive = new List<int>(8);
    public List<bool> quickReceiveIsEquipping = new List<bool>(8);

    [HideInInspector]
    public int money = 0;


    public int Add(Item item)
    {
        //아이템이 이미 포함되어있거나 장비아이템이 아닐때
        if (items.Contains(item) && item.itemType != Item.ItemType.Equipment)
        {
            onQuantityChangedCallback.Invoke(item, FIndInventoryIndex(item), 1);
            return FIndInventoryIndex(item);
        }

        int index = GetEmptyIndex();

        //아이템이 이미 인벤토리에 있을 경우
        //장비는 이미 인벤토리에 있더라도 한칸당 한개의 장비 아이템을 갖는걸 원칙으로 함.
        
        
        //인벤토리가 가득 차있을 경우
        if (IsFull())
        {
            return -1;
        }
        items[index] = item;

        onItemChangedCallback.Invoke(index);
        onQuantityChangedCallback.Invoke(item, index, 1);

        
        return index;
    }
    public bool Add(Item item, int index)
    {
        //인벤토리가 가득 차있을 경우
        if (IsFull() || items[index] != null)
        {
            return false;
        }

        //아이템이 이미 인벤토리에 있을 경우
        //장비는 이미 인벤토리에 있더라도 한칸당 한개의 장비 아이템을 갖는걸 원칙으로 함.

        if (items.Contains(item) && item.itemType != Item.ItemType.Equipment) //&& item.itemType != Item.ItemType.Equipment)
        {
            onQuantityChangedCallback.Invoke(item, FIndInventoryIndex(item), 1);
            
            return true;
        }

        items[index] = item;
        onItemChangedCallback.Invoke(index);
        onQuantityChangedCallback.Invoke(item, index, 1);
        return true;
    }


    public void Remove(Item item)
    {
        int index = items.IndexOf(item);
        items[index] = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                return;
            }
        }
    }

    public bool IsFull()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                return false;
            }
        }
        return true;
    }

    public int GetEmptyIndex()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public int FIndInventoryIndex(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                return i;
            }
        }
        return -1;
    }

    //장비창에 해당 아이템을 검색 없으면 -1 반환
    
    
    public bool ChangeMoney(int val)
    {
        //돈을 깎는 경우
        if(val < 0)
        {
            //돈이 깎이는 값보다 작을떄
            if(money < -val)
            {
                return false;
            }
        }
        money += val;
        onOnUpdateMoneyCallback();
        return true;
    }

    public void TradeInvenToDrag(Item moveItem, int index,int moveQuantity)
    {
        Item tempItem = moveItem;
        int tempQuantity = moveQuantity;

        items[index] = DragSlot.instance.item;
        itemQuantity[index] = DragSlot.instance.quantity;

        items[DragSlot.instance.index] = tempItem;
        itemQuantity[DragSlot.instance.index] = tempQuantity;


        if(tempItem == null)
        {
            QuickReceiveIndexOfChange(DragSlot.instance.index, index);
        }
        else if(tempItem != null)
        {
            if (quickReceive.IndexOf(DragSlot.instance.index) != -1)
            {
                QuickReceiveIndexOfChange(DragSlot.instance.index, index);

                QuickReceiveIndexOfChange(index, DragSlot.instance.index);
            }
        }
        inventorySlots[DragSlot.instance.index].UpdateSlot();
        inventorySlots[index].UpdateSlot();
    }


    public void QuickReceiveIndexOfChange(int index, int changeIndex)
    {
        if(quickReceive.IndexOf(index) != -1)
        { 
            quickReceive[quickReceive.IndexOf(index)] = changeIndex;
        }
        
    }

    public void QuickReceiveForEquipmentChange(int index, int changeIndex, bool isEquipping, bool changeEquipping)
    {

        if(index == -1)
        {
            return;
        }
        
        for(int i=0; i<quickReceive.Count; i++)
        {
            if (quickReceive[i] == index && quickReceiveIsEquipping[i] == isEquipping)
            {
                quickReceive[i] = changeIndex;
                quickReceiveIsEquipping[i] = changeEquipping;
            }
        }
        
    }
}
