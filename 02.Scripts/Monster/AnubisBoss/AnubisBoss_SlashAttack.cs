using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnubisBoss_SlashAttack : MonoBehaviour
{
    public SphereCollider attackCollider;
    public AnubisBoss_Attack attackScript;
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("PLAYER"))
        {
            Player_HP_Stamina.instance.DecreaseHp(attackScript.atk);
            attackCollider.enabled = false;
        }
    }
}
