using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Effect : MonoBehaviour
{
    public Transform effectPosition;

    [HideInInspector]
    public Player_Movement playerMovement;

    public ParticleSystem healEffect, staminaEffect, meleeAttackEffect, poisonEffect, poisonHeadEffect, swordSlashEffect, freezePrison;


    private float[] randomXYZ = new float[3];

    private float playerSlowSpeed = 2f, slowCount = 0;


    #region Singleton
    public static Player_Effect instance;

    void Awake()
    {

        playerMovement = GetComponent<Player_Movement>();

        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    #endregion
    public void PlayEffect(ParticleSystem effect)
    {
        effect.gameObject.SetActive(true);
        effect.Play();
    }   
    public void StopEffect(ParticleSystem effect)
    {
        effect.Stop();
        effect.gameObject.SetActive(false);
    }
    public IEnumerator PlayLoopEffectCourutine(ParticleSystem effect, float time)
    {
        effect.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        effect.gameObject.SetActive(false);
        yield break;
    } 


    public void HitEffect(Vector3 targetPosition)
    {
        for (int i = 0; i < randomXYZ.Length; i++)
        {
            randomXYZ[i] = UnityEngine.Random.Range(0f, 0.2f);
        }
        Vector3 effectPosition = new Vector3(targetPosition.x + randomXYZ[0], targetPosition.y + randomXYZ[1], targetPosition.z + randomXYZ[2]);

        meleeAttackEffect.transform.position = effectPosition;
        meleeAttackEffect.transform.GetChild(0).position = effectPosition;
        meleeAttackEffect.gameObject.SetActive(true);
        meleeAttackEffect.transform.GetChild(0).gameObject.SetActive(true);

    }

    #region Poison For Spider
    public IEnumerator PoisonAttacked()
    {
        if (playerMovement.slowCount < 3)
        {
            instance.slowCount++;
            playerMovement.SetSpeed(playerSlowSpeed, playerSlowSpeed * 2);
            PlayEffect(poisonEffect);
            PlayEffect(poisonHeadEffect);
            yield return new WaitForSeconds(3f);
        }
        else if (playerMovement.slowCount >= 3)
        {
            yield break;
        }
        StopPoison();
    }

    public void StopPoison()
    {
        if (slowCount > 1)
        {
            slowCount--;
            return;
        }
        slowCount--;
        playerMovement.SetSpeed(playerMovement.walkSpeed, playerMovement.runSpeed);
        StopEffect(poisonEffect);
        StopEffect(poisonHeadEffect);
    }
    #endregion

}
