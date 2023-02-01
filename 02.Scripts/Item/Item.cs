using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    private string cashSFX = "Item_Cash";
    //장비, 사용, 재료, 기타
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

    public int itemCode;
    public string korName;
    public string engName;
    [TextArea]
    public string itemDesc;
    public ItemType itemType;
    public int itemPrice;
    public Sprite itemImage;

    public virtual void Use(int index)
    {

    }

    public void Buy()
    {
        bool canBuy = Inventory.instance.ChangeMoney(-itemPrice);
        if(canBuy)
        {
            Inventory.instance.Add(this);
            AudioManager.instance.SFXPlay(cashSFX);
        }
    }

    public void Sell(int index)
    {
        Inventory.instance.ChangeMoney(itemPrice);
        Inventory.instance.onQuantityChangedCallback.Invoke(this, index, -1);
        AudioManager.instance.SFXPlay(cashSFX);

    }

}
