using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "New Item/Potion")]
public class PotionItem : Item
{
    public enum PotionType
    {
        HealthPotion,
        StaminaPotion
    }
    //장비, 사용, 재료, 기타
    public PotionType potionType;
    public int value;
    private string healthPotionSound = "PotionItem_HealthPotionSound", staminaPotionsound = "PotionItem_StaminaPotionSound";
    
    public override void Use(int index)
    {
        bool isUsePoition;
        if (potionType == PotionType.HealthPotion)
        {
            isUsePoition = Player_HP_Stamina.instance.IncreaseHp(value);
            if(isUsePoition)
            {
                Player_Effect.instance.PlayEffect(Player_Effect.instance.healEffect);
                Inventory.instance.onQuantityChangedCallback.Invoke(this, index, -1);
                AudioManager.instance.SFXPlay(healthPotionSound);
            }
        }
        else if (potionType == PotionType.StaminaPotion)
        {
            isUsePoition = Player_HP_Stamina.instance.IncreaseStamina(value);

            if (isUsePoition)
            {
                Player_Effect.instance.PlayEffect(Player_Effect.instance.staminaEffect);
                Inventory.instance.onQuantityChangedCallback.Invoke(this, index, -1);
                AudioManager.instance.SFXPlay(staminaPotionsound);
            }
            
        }
    }
}
