using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Player_Equipment : MonoBehaviour
{
    public EquipmentSlot[] equipSlots = new EquipmentSlot[6];
    public WeaponItem weapon;

    public Transform leftHand, rightHand, player;
    #region Singleton
    public static Player_Equipment instance;

    [HideInInspector]
    public Player_Attack playerAttack;

    [HideInInspector]
    public Player_Movement playerMovement;



    void Awake()
    {
        playerAttack = GetComponent<Player_Attack>();
        playerMovement = GetComponent<Player_Movement>();

        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    #endregion

    public int Equip(EquipmentItem newItem)
    {
        //머리 장비 착용
        int slotIndex = (int)newItem.equipType;
        int index = slotIndex;
        weapon = (WeaponItem)newItem;
        playerAttack.minAtk = weapon.minAtk;
        playerAttack.maxAtk = weapon.maxAtk;

        //양손무기는 무기칸에
        if (slotIndex == 6) index = 1;

        //기존에 장비 아이템이 착용되어있을 시에
        if (equipSlots[index].equipmentItem != null)
        {
            UnEquip(index);
        }

        
        
        //기존 장비 아이템이 비어있을때


        //한손 무기 장착 시
        if (slotIndex == 1)
        {
            //장착 시 이미 다른 무기가 장착되어있을 경우 비활성화
            if (equipSlots[1] != null)
            {
                RightHandEmpty();
            }
            if (equipSlots[2] != null)
            {
                LeftHandEmpty();
            }
            playerAttack.isAttacking = false;
            rightHand.gameObject.SetActive(true);
            rightHand.Find(newItem.engName).gameObject.SetActive(true);

            weapon = (WeaponItem)newItem;
        }

        //쉴드 장착 시
        else if(slotIndex == 2)
        {
            if (equipSlots[2] != null)
            {
                LeftHandEmpty();
            }
            leftHand.gameObject.SetActive(true);
            leftHand.Find(newItem.engName).gameObject.SetActive(true);
        }
        
        //두손 무기 장착 시
        else if(slotIndex == 6)
        {
            if (equipSlots[1] != null)
            {
                RightHandEmpty();
            }

            LeftHandEmpty();
            playerAttack.isAttacking = false;
            leftHand.gameObject.SetActive(true);
            leftHand.Find(newItem.engName).gameObject.SetActive(true);


            //장착 무기가 활일때 
            if (weapon.itemCode == 6)
            {
                ChangeBow(true, false);
            }

            //활은 일반 모션과 당기는 모션이 있으니 첫번째 오브젝트는 활성화
            //두번째 오브젝트는 활을 당기고 있을때 모델이므로 비활성화
        }
        equipSlots[index].Add(newItem);
        return index;
    }
    public void ChangeBow(bool one, bool two)
    {
        leftHand.Find("Bow").transform.GetChild(0).gameObject.SetActive(one);
        leftHand.Find("Bow").transform.GetChild(1).gameObject.SetActive(two);
    }

    public void UnEquip(int slotIndex)
    {
        //활을 장비하고 있을때 다른 장비를 착용
        if (weapon.weaponType == WeaponItem.WeaponType.Bow)
        {
            playerAttack.EndZoom();
        }
        if (slotIndex == 6)
        {
            LeftHandEmpty();
            RightHandEmpty();
            slotIndex = 1;
        }

        else if (slotIndex == 1)
        {
            RightHandEmpty();
        }
        

            Inventory.instance.QuickReceiveForEquipmentChange(slotIndex, Inventory.instance.Add(equipSlots[slotIndex].equipmentItem), true, false);
        equipSlots[slotIndex].Remove();
    }

    public void UnEquip(int slotIndex, int inventoryIndex)
    {
        if (weapon.weaponType == WeaponItem.WeaponType.Bow)
        {
            playerAttack.EndZoom();
        }

        if (equipSlots[slotIndex] != null)
        {
            if (slotIndex == 1)
            {
                RightHandEmpty();
            }
            else if (slotIndex == 6)
            {
                slotIndex = 1;
                LeftHandEmpty();
                RightHandEmpty();
            }

            Inventory.instance.Add(equipSlots[slotIndex].equipmentItem, inventoryIndex);
            Inventory.instance.QuickReceiveForEquipmentChange(slotIndex, inventoryIndex, true, false);
            equipSlots[slotIndex].Remove();
        }
    }

    public void UnEquipAll()
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            UnEquip(i);
        }
    }

    public void RightHandEmpty()
    {
        foreach (Transform rightChild in rightHand.GetComponentsInChildren<Transform>())
        {
            rightChild.gameObject.SetActive(false);
        }
    }
    public void LeftHandEmpty()
    {
        foreach (Transform leftChild in leftHand.GetComponentsInChildren<Transform>())
        {
            leftChild.gameObject.SetActive(false);
        }
    }

    //해당 장비를 착용하고 있는지 검사
    public int FindEquipmentIndex(EquipmentItem eq)
    {
        for(int i=0; i<equipSlots.Length; i++)
        {
            if (equipSlots[i].equipmentItem == eq) return i;
        }
        return -1;
    }
}
