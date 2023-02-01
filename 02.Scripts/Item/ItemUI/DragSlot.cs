using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    [HideInInspector]
    public Item item;
    [HideInInspector]
    public int index, quantity = 0;


    public enum InventoryType
    {
        Normal,
        Quick,
        Equipment,
        Shop,
        Null
    }
    public InventoryType dragBeginType;
    public InventoryType dragEndType;

    public void BeginDrag(Item item, int index, int quantity, InventoryType beginType)
    {
        this.item = item;
        this.index = index;
        this.quantity = quantity;
        dragBeginType = beginType;
    }

    public void EndDrag()
    {
        this.item = null;
        this.index = -1;
        this.quantity = -1;
        dragBeginType = InventoryType.Null;
        dragEndType = InventoryType.Null;
    }
}
