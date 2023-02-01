using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "New Item/Equipment")]
public class EquipmentItem : Item
{ 
    public enum EquipmentType
    {
        Head = 0,
        OneHandWeapon = 1,
        Shield = 2,
        Chest = 3,
        Legs = 4,
        Shoes = 5,
        TwoHandWeapon = 6
    }
    public EquipmentType equipType;


    //index는 장착 중이 아닐떄는 인벤토리 인덱스, 장착 중일때는 Player_Equipment에서 장비 인덱스)
    public override void Use(int index)
    {
        //장비를 착용이 아닐때
        if (Inventory.instance.FIndInventoryIndex(this) != -1)
        {
            int tempEquipType = (int)equipType;

            if ((int)equipType == 6) tempEquipType = 1;

            //아이템을 장착후에 
            int equipindex = Player_Equipment.instance.Equip(this);

            Inventory.instance.QuickReceiveForEquipmentChange(index, equipindex, false, true);
            Inventory.instance.onQuantityChangedCallback.Invoke(this, index, -1);
        }
        else if (Player_Equipment.instance.FindEquipmentIndex(this) != -1)
        {
            Inventory.instance.QuickReceiveIndexOfChange(index, Inventory.instance.GetEmptyIndex());
            Player_Equipment.instance.UnEquip((int)equipType);
        }
        else
        {
            return;
        }
    }
}
