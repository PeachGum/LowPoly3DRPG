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
        //�÷��̾� ������, ���� ����
        Player_Effect.instance.playerMovement.canMove = false;
        Player_Equipment.instance.playerAttack.canAttack = false;
        
        //��, �� ���� �׵θ��� ���� �ڷ�ƾ
        StartCoroutine(LerpForUI(0, 1));
        
        canvasSide.SetActive(true);
        //��ȯ �ƿ츮 ����Ʈ ��Ȱ��ȭ
        summonAura.SetActive(false);
        //��ȯ�ϱ� ���� �ݶ��̴� ��Ȱ��ȭ
        summonZone.enabled = false;
        //������ ���� ��Ż ��Ȱ��ȭ
        villagePortal.SetActive(false);
        //������ �÷��̾� ���� ī�޶� ��Ȱ��ȭ�ϰ� ���� ������� ���� ī�޶� Ȱ��ȭ
        camManager = GameObject.FindObjectOfType<CameraManager>();
        camManager.movingCamera.enabled = false;
        bossSummonCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        //���� �ı�
        AudioManager.instance.SFXPlay(rockDestroySFX); 
        stoneStatue.SetActive(false);
        ParticlePlay(destoryStatue);
        ParticlePlay(destoryStatue2);

        yield return new WaitForSeconds(2f);

        //��ȯ���϶� ����Ʈ Ȱ��ȭ
        AudioManager.instance.SFXPlay(summonChargingSFX);
        ParticlePlay(chargingSummonEffect);
        ParticlePlay(chargingSummonEffect2);

        yield return new WaitForSeconds(chargingTime);

        //��ȯ�� ����Ʈ�� ��Ȱ��ȭ�ϰ�
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
