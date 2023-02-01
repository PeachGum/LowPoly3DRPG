using PolygonArsenal;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnubisBoss_Attack : MonoBehaviour
{
    private GameObject attackTarget;
    [HideInInspector]
    public AnubisBoss_Moving moving;
    public AnubisBoss_SlashAttack slashAttackScript;

    public ParticleSystem turnAttackEffect, slashEffect, slashEndGroundNova, freezeMagicEffect, freezeMagicStart, orbitalMagicCharging;
    public List<ParticleSystem> orbitalBeam;

    public float atk = 2f;
    private float turnAttackRange = 3.7f, speed;
    [HideInInspector]
    public bool isAttacking;
    private int randomInt;

    private string slashAttack = "Boss_SlashAttack";
    private string slashAttackEnd = "Boss_SlashAttackEnd";
    private string freezeMagicCharging = "Boss_FreezeMagicCharging";
    private string freezeMagicMoment = "Boss_FreezeMagicMoment";
    private string orbitalBeammagic_charging = "Boss_OrbitalBeamMagic_Charging";
    private string turnAttackSFX = "Boss_TurnAttack";

    public readonly int hashSlashAttack = Animator.StringToHash("SlashAttack");
    public readonly int hashTurnAttack = Animator.StringToHash("TurnAttack");
    // Start is called before the first frame update
    void Start()
    {
        moving = transform.parent.GetComponent<AnubisBoss_Moving>();

        speed = moving.nav.speed;

    }

    private void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("PLAYER") && !moving.nav.isStopped && !isAttacking)
        {
            
            attackTarget = col.gameObject;
            randomInt = Random.Range(1, 3);
            Debug.Log($"Attack : {randomInt}");
            if (randomInt == 1)
            {
                moving.anim.SetTrigger(hashSlashAttack);
            }

            else if (randomInt == 2)
            {
                moving.anim.SetTrigger(hashTurnAttack);
            }
            
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            attackTarget = null;
        }
    }
    #region TurnAtktack
    public void TurnAttack()
    {
        moving.summon.ParticlePlay(turnAttackEffect);
        AudioManager.instance.SFXPlay(turnAttackSFX);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, turnAttackRange);
        foreach (var col in hitEnemies)
        {
            if (col.gameObject.CompareTag("PLAYER"))
            {
                Player_HP_Stamina.instance.DecreaseHp(atk);
            }
        }
    }
    #endregion

    #region SlashAttack
    public void SlashAttackStart()
    {
        isAttacking = true;
        moving.nav.speed = 0.5f;
        moving.anim.SetBool(moving.hashIsWalking, false);
    }

    public void SlashAttackForSFX()
    {
        AudioManager.instance.SFXPlay(slashAttack);
        moving.summon.ParticlePlay(slashEffect);

    }

    public void SlashAttackMoment()
    {
        AudioManager.instance.SFXPlay(slashAttackEnd);
        slashAttackScript.attackCollider.enabled = true;
        moving.summon.ParticlePlay(slashEndGroundNova);
        //Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        //foreach (var targets in hitEnemies)
        //{
        //    if (targets.gameObject.CompareTag("PLAYER"))
        //    {
        //        Player_HP_Stamina.instance.DecreaseHp(atk);
        //    }
        //}
    }
    public void SlashAttackColliderOff()
    {
        slashAttackScript.attackCollider.enabled = false;
    }
    public void SlashAttackEnd()
    {
        isAttacking = false;
        moving.nav.speed = speed;

        if (moving.reservationRandomAnimation)
        {
            moving.RandomAnimation();
            moving.reservationRandomAnimation = false;
            return;
        }
        moving.anim.SetBool(moving.hashIsWalking, true);
    }
    #endregion

    #region FreezeMagic

    public void FreezeMagicStart()
    {
        if (isAttacking)
        {
            isAttacking = false;
            return;
        }
        freezeMagicStart.gameObject.SetActive(true);
        AudioManager.instance.SFXPlay(freezeMagicCharging);
        moving.nav.isStopped = true;
        moving.anim.SetBool(moving.hashIsWalking, false);
    }

    public void FreezeMagicMoment()
    {
        AudioManager.instance.SFXPlay(freezeMagicMoment);
        moving.summon.ParticlePlay(freezeMagicEffect);

    }
    public void FreezeMagicEnd()
    {
        isAttacking = false;
        moving.anim.SetBool(moving.hashIsWalking, true);
        StartCoroutine(FreezeMagicEndCoroutine());   
    }

    public IEnumerator FreezeMagicEndCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        moving.nav.isStopped = false;

        int randomInt = Random.Range(7, 10);
        yield return new WaitForSeconds(randomInt);
        moving.RandomAnimation();
    }
    #endregion

    #region OrbitalBeam
    public void OrbitalBeamStart()
    {
        if (isAttacking)
        {
            isAttacking = false;
            return;
        }
        orbitalMagicCharging.gameObject.SetActive(true);
        AudioManager.instance.SFXPlay(orbitalBeammagic_charging);
        moving.nav.isStopped = true;
        moving.anim.SetBool(moving.hashIsWalking, false);
    }

    public void OrbitalBeamEnd()
    {
        isAttacking = false;
        moving.anim.SetBool(moving.hashIsWalking, true);
        StartCoroutine(OrbitalBeamCoroutine());
        StartCoroutine(FreezeMagicEndCoroutine());
    }

    IEnumerator OrbitalBeamCoroutine()
    {
        foreach(var beam in orbitalBeam)
        {
            
            float randomX = Random.Range(-9, 9);
            float randomZ = Random.Range(-7, 11);
            Vector3 beamPosition = new Vector3(randomX, 0, randomZ);
            beam.transform.position = beamPosition;
            beam.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);
        }

        yield break;
    }
    #endregion

}

