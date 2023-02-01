using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Player_SwordSpecialEffect : MonoBehaviour
{
    private ParticleSystem particle;
    private string swordHit = "Player_SwordHit";
    private float attackRange = 2.6f;
    WaitForSeconds firstWait = new WaitForSeconds(0.5f);
    WaitForSeconds secondWait = new WaitForSeconds(0.18f);

    private void OnEnable()
    {
        if(particle == null)
        {
            particle = GetComponent<ParticleSystem>();
        }
        particle.Play();
        StartCoroutine(SpecialAttack());
    }

    IEnumerator SpecialAttack()
    {
        yield return firstWait;
        Attack();
        yield return secondWait;
        Attack();
        yield return secondWait;
        Attack();
        yield return secondWait;
        Attack();
        yield return secondWait;
        Attack();
        yield return secondWait;
        Attack();
        yield return secondWait;
        gameObject.SetActive(false);

    }
    void Attack()
    {
        AudioManager.instance.SFXPlay(swordHit);
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        foreach (var targets in hitEnemies)
        {
            if (targets.gameObject.CompareTag("MONSTER"))
            {
                Monster_Stat targetStat = targets.GetComponent<Monster_Stat>();
                if (targetStat.nowHp > 0)
                {
                    targetStat.SetHealth(-Random.Range(Player_Equipment.instance.weapon.minAtk, Player_Equipment.instance.weapon.maxAtk));
                    targetStat.movingMonster.target = transform;
                    //HitEffect(col.transform.position);
                }
            }
        }
    }
}
