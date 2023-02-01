using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnubisBoss_Summon : MonoBehaviour
{
    private string summonChargingSFX = "Boss_SummonCharging", summonSFX = "Boss_Summon", rockDestroySFX = "RockDestroy";
    public Camera bossSummonCamera;
    public GameObject stoneStatue, boss, summonAura, villagePortal;
    private GameObject canvasSide;
    private BoxCollider summonZone;
    public ParticleSystem destoryStatue, destoryStatue2, chargingSummonEffect, chargingSummonEffect2, summonEffect;
    public Canvas bossCanvas;
    private Image upSide, downSide;

    CameraManager camManager;
    float chargingTime = 5f, startTime, deltaTime, totalTime = 1f;

    AnubisBoss_Stat boss_Stat;

    private void Start()
    {
        upSide = bossCanvas.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        downSide = bossCanvas.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        canvasSide = bossCanvas.transform.GetChild(0).gameObject;
        summonZone = GetComponent<BoxCollider>();
    }

    public IEnumerator SummonBoss()
    {
        //플레이어 움직임, 공격 봉인
        Player_Effect.instance.playerMovement.canMove = false;
        Player_Equipment.instance.playerAttack.canAttack = false;
        
        //상, 하 검은 테두리를 위한 코루틴
        StartCoroutine(LerpForUI(0, 1));
        
        canvasSide.SetActive(true);
        //소환 아우리 이펙트 비활성화
        summonAura.SetActive(false);
        //소환하기 위한 콜라이더 비활성화
        summonZone.enabled = false;
        //마을로 가는 포탈 비활성화
        villagePortal.SetActive(false);
        //기존의 플레이어 추적 카메라를 비활성화하고 보스 등장씬을 위한 카메라 활성화
        camManager = GameObject.FindObjectOfType<CameraManager>();
        camManager.movingCamera.enabled = false;
        bossSummonCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        //석상 파괴
        AudioManager.instance.SFXPlay(rockDestroySFX); 
        stoneStatue.SetActive(false);
        ParticlePlay(destoryStatue);
        ParticlePlay(destoryStatue2);

        yield return new WaitForSeconds(2f);

        //소환중일때 이펙트 활성화
        AudioManager.instance.SFXPlay(summonChargingSFX);
        ParticlePlay(chargingSummonEffect);
        ParticlePlay(chargingSummonEffect2);

        yield return new WaitForSeconds(chargingTime);

        //소환중 이펙트를 비활성화하고
        AudioManager.instance.SFXStop(summonChargingSFX);
        chargingSummonEffect.gameObject.SetActive(false);
        chargingSummonEffect2.gameObject.SetActive(false);
        AudioManager.instance.SFXPlay(summonSFX);
        ParticlePlay(summonEffect);
        boss.SetActive(true);
        boss_Stat = boss.GetComponent<AnubisBoss_Stat>();

        yield break;
    }

    public void RoarEnd()
    {
        Player_Effect.instance.playerMovement.canMove = true;
        Player_Equipment.instance.playerAttack.canAttack = true;
        StartCoroutine(LerpForUI(1, 0));
    }

    public IEnumerator LerpForUI(float from, float to)
    {
        startTime = Time.time;

        do
        {
            deltaTime = Time.time - startTime;
            deltaTime *= 2;
            upSide.fillAmount = Mathf.Lerp(from, to, deltaTime / totalTime);
            downSide.fillAmount = Mathf.Lerp(from, to, deltaTime / totalTime);
            yield return null;
        } while (deltaTime < totalTime);

        if(from == 1)
        {
            camManager.movingCamera.enabled = true;
            bossSummonCamera.gameObject.SetActive(false);
            boss_Stat.boss_hpCanvas.SetActive(true);
            boss_Stat.boss_hpFill.fillAmount = 1f;
            boss_Stat.boss_HpText.text = boss_Stat.maxHp.ToString();
        }

    }
    public void ParticlePlay(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);
        particle.Play();
    }

    public void BossDie()
    {
        villagePortal.SetActive(true);
    }
}
