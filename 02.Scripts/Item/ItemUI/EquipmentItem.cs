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


    //index�� ���� ���� �ƴҋ��� �κ��丮 �ε���, ���� ���϶��� Player_Equipment���� ��� �ε���)
    public override void Use(int index)
    {
        //��� ������ �ƴҶ�
        if (Inventory.instance.FIndInventoryIndex(this) != -1)
        {
            int tempEquipType = (int)equipType;

            if ((int)equipType == 6) tempEquipType = 1;

            //�������� �����Ŀ� 
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
