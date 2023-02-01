using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalBeamScript : MonoBehaviour
{
    private const string charging = "Boss_OrbitalBeamCharging", boom = "Boss_OrbitalBeamBoom";
    WaitForSeconds chargingWait, damageWait;
    private float attackRange = 1.3f;

    private void Awake()
    {
        chargingWait = new WaitForSeconds(2f);
        damageWait = new WaitForSeconds(0.05f);


    }
    private void OnEnable()
    {
        StartCoroutine(OrbitalBeam());
    }

    IEnumerator OrbitalBeam()
    {
        AudioManager.instance.SFXPlay(charging);
        yield return chargingWait;
        AudioManager.instance.SFXPlay(boom);
        
        for (int i = 0; i < 30; i++)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var col in hitEnemies)
            {
                if (col.CompareTag("PLAYER"))
                {
                    Player_HP_Stamina.instance.DecreaseHp(0.5f);
                }
                
            }
            yield return damageWait;
        }
        
    }
}
