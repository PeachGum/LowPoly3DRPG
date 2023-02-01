using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnubisBoss_Stat : Monster_Stat
{
    private string shieldMagic = "Boss_ShieldMagic", shielding = "Boss_Shielding", bossDieSound = "Boss_Die";
    public readonly int hashDieBool = Animator.StringToHash("DieBool");
    public ParticleSystem defenseEffect;
    public bool defenseBuff;
    private float defenseBuffTime = 6f;

    AnubisBoss_Attack attack;

    public GameObject boss_hpCanvas;
    [HideInInspector]
    public Image boss_hpFill;
    [HideInInspector]
    public TextMeshProUGUI boss_HpText;
    private void Awake()
    {
        Init();
        boss_hpFill = boss_hpCanvas.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        boss_HpText = boss_hpCanvas.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        movingMonster = GetComponent<AnubisBoss_Moving>();
        attack = transform.GetChild(5).GetComponent<AnubisBoss_Attack>();
        dieSound = bossDieSound;
    }

    private void OnEnable()
    {
        RespawnMonster();
    }
    // Start is called before the first frame update

    public override void SetHealth(int health)
    {
        if(!slider.gameObject.activeSelf)
        {
            slider.gameObject.SetActive(true);
        }
        int calHealth = health;
        if(defenseBuff && health < 0)
        {
            calHealth = Mathf.RoundToInt(calHealth * 0.5f);
        }

        if (nowHp > 0)
        {
            nowHp += health;
            slider.value = nowHp;
            hpText.text = nowHp.ToString();
            if (health < 0)
            {
                skinnedMeshRenderer.material.color = hitColor;
                //데미지 글짜 표시
                if (memoryPool != null)
                {
                    memoryPool.CreatePoolForText(transform, (health * -1).ToString());
                }
                Invoke("RecoveryColor", 0.2f);
            }

        }

        boss_hpFill.fillAmount = nowHp / (float)maxHp;
        if (nowHp < 0) nowHp = 0;
        boss_HpText.text = nowHp.ToString();

        if(nowHp <= 0 && slider.gameObject.activeSelf)
        {
            StopAllCoroutines();
            attack.StopAllCoroutines();
            attack.moving.StopAllCoroutines();
            AudioManager.instance.SFXStop(shielding);
            movingMonster.target = null;
            boss_hpCanvas.SetActive(false);

            isDie = true;
            DieSound();
            DropItem();
            slider.gameObject.SetActive(false);
            movingMonster.anim.SetTrigger(hashDie);
            movingMonster.nav.speed = 0;
            movingMonster.nav.isStopped = true;

            attack.moving.summon.BossDie();
            StartCoroutine(DisableMonster());
        }
    }
    #region DefenseBuff
    void DefenseBuffStart()
    {
        if (attack.isAttacking)
        {
            return;
        }

        
        AudioManager.instance.SFXPlay(shieldMagic);
        movingMonster.nav.isStopped = true;
        attack.moving.anim.SetBool(attack.moving.hashIsWalking, false);
    }
    IEnumerator DefenseBuff_Courutine()
    {
        defenseBuff = true;
        defenseEffect.gameObject.SetActive(true);
        defenseEffect.Play();
        AudioManager.instance.SFXPlay(shielding);

        yield return new WaitForSeconds(0.5f);
        movingMonster.nav.isStopped = false;
        
        yield return new WaitForSeconds(defenseBuffTime);

        defenseBuff = false;
        defenseEffect.gameObject.SetActive(false);
        AudioManager.instance.SFXStop(shielding);
        defenseEffect.Stop();
    }
    void DefenseBuffEnd()
    {
        attack.isAttacking = false;
        attack.moving.anim.SetBool(attack.moving.hashIsWalking, true);
        StartCoroutine(DefenseBuff_Courutine());
        StartCoroutine(DefenseBuffStop());
        
    }
    IEnumerator DefenseBuffStop()
    {
        int randomInt = Random.Range(6, 10);
        yield return new WaitForSeconds(randomInt);
        movingMonster.RandomAnimation();
    }
    #endregion

}
