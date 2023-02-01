using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Item/Weapon")]
public class WeaponItem : EquipmentItem
{
    // Start is called before the first frame update
    public enum WeaponType
    {
        Sword = 0,
        Bow = 1,
        Gun = 2,

    }

    public WeaponType weaponType;
    public int minAtk;
    public int maxAtk;

}
