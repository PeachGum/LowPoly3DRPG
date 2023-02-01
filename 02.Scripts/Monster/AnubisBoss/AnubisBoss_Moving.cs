using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnubisBoss_Moving : Monster_Moving
{
    private int randomInt;
    private string roarSFX = "Boss_Roar";
    public bool reservationRandomAnimation = false;


    public readonly int hashDefenceSpell = Animator.StringToHash("DefenceSpell");
    public readonly int hashFreezeMagic = Animator.StringToHash("FreezeMagic");
    public readonly int hashOrbitalBeam = Animator.StringToHash("OrbitalBeam");
    public readonly int hashSummon = Animator.StringToHash("Summon");
    public readonly int hashIsWalking = Animator.StringToHash("isWalking");

    public AnubisBoss_Summon summon;
    AnubisBoss_Attack attack;
    AnubisBoss_Stat stat;

    bool isSummoned;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("PLAYER").transform;
        attack = transform.GetChild(5).GetComponent<AnubisBoss_Attack>();
        stat = GetComponent<AnubisBoss_Stat>();


    }
    void OnEnable()
    {
        if(anim == null)
        {
            Init();
        }
        
        nav.isStopped = true;
        StartCoroutine(OnEnableCoroutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectionPlayer();
    }

    public override void ResetMonster()
    {
        //nav.isStopped = true;
        return;
    }
    public override void RandomAnimation()
    {
        if(attack.isAttacking && !reservationRandomAnimation)
        {

            reservationRandomAnimation = true;
            return;
        }
        randomInt = Random.Range(1, 4);
        //경로를 가지고 있지 않을때
        //방어 버프
        if (randomInt == 1)
        {
            anim.SetTrigger(hashDefenceSpell);
        }
        //플레이어 추적
        if (randomInt == 2)
        {
            anim.SetTrigger(hashFreezeMagic);
        }
        if (randomInt == 3)
        {
            anim.SetTrigger(hashOrbitalBeam);
        }

    }
    public void DetectionPlayer()
    {
        if(!nav.isStopped && target != null && !stat.isDie)
        {
            nav.SetDestination(target.position);
            if(!anim.GetBool(hashIsWalking) && !attack.isAttacking)
            {
                anim.SetBool(hashIsWalking, true);
            }
        }
    }

    void RoarStart()
    {
        AudioManager.instance.SFXPlay(roarSFX);
    }
    void RoarEnd()
    {
        if (!isSummoned)
        {
            summon.RoarEnd();
            StartCoroutine(RoarEndCoroutine());
            isSummoned = true;
        }
        else
        {
            anim.SetBool(hashIsWalking, false);
            target = null;
        }
        
        
    }
    IEnumerator OnEnableCoroutine()
    {
        //소환되고 1초후에 함성을 지름
        yield return new WaitForSeconds(1f);
        anim.SetTrigger(hashSummon);
    }
    IEnumerator RoarEndCoroutine()
    {
        //함성을 지르고 2초뒤에 랜덤으로 방어 버프 또는 얼음 공격을 함
        yield return new WaitForSeconds(2f);
        RandomAnimation();
    }


    #region Anmation_Attack
    void SlashAttackStart()
    {
        attack.SlashAttackStart();
    }

    void SlashAttackForSFX()
    {
        attack.SlashAttackForSFX();

    }

    void SlashAttackMoment()
    {
        attack.SlashAttackMoment();
    }

    void SlashAttackColliderOff()
    {
        attack.SlashAttackColliderOff();
    }
    void SlashAttackEnd()
    {
        attack.SlashAttackEnd();
    }

    void FreezeMagicStart()
    {
        attack.FreezeMagicStart();
    }

    void FreezeMagicMoment()
    {
        attack.FreezeMagicMoment();

    }
    void FreezeMagicEnd()
    {
        attack.FreezeMagicEnd();
    }

    void OrbitalBeamStart()
    {
        attack.OrbitalBeamStart();
    }

    void OrbitalBeamEnd()
    {
        attack.OrbitalBeamEnd();
    }

    void TurnAttack()
    {
        attack.TurnAttack();
    }

    #endregion
}
