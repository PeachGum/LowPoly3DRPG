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
        //�Ӹ� ��� ����
        int slotIndex = (int)newItem.equipType;
        int index = slotIndex;
        weapon = (WeaponItem)newItem;
        playerAttack.minAtk = weapon.minAtk;
        playerAttack.maxAtk = weapon.maxAtk;

        //��չ���� ����ĭ��
        if (slotIndex == 6) index = 1;

        //������ ��� �������� ����Ǿ����� �ÿ�
        if (equipSlots[index].equipmentItem != null)
        {
            UnEquip(index);
        }

        
        
        //���� ��� �������� ���������


        //�Ѽ� ���� ���� ��
        if (slotIndex == 1)
        {
            //���� �� �̹� �ٸ� ���Ⱑ �����Ǿ����� ��� ��Ȱ��ȭ
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

        //���� ���� ��
        else if(slotIndex == 2)
        {
            if (equipSlots[2] != null)
            {
                LeftHandEmpty();
            }
            leftHand.gameObject.SetActive(true);
            leftHand.Find(newItem.engName).gameObject.SetActive(true);
        }
        
        //�μ� ���� ���� ��
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


            //���� ���Ⱑ Ȱ�϶� 
            if (weapon.itemCode == 6)
            {
                ChangeBow(true, false);
            }

            //Ȱ�� �Ϲ� ��ǰ� ���� ����� ������ ù��° ������Ʈ�� Ȱ��ȭ
            //�ι�° ������Ʈ�� Ȱ�� ���� ������ ���̹Ƿ� ��Ȱ��ȭ
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
        //Ȱ�� ����ϰ� ������ �ٸ� ��� ����
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

    //�ش� ��� �����ϰ� �ִ��� �˻�
    public int FindEquipmentIndex(EquipmentItem eq)
    {
        for(int i=0; i<equipSlots.Length; i++)
        {
            if (equipSlots[i].equipmentItem == eq) return i;
        }
        return -1;
    }
}
