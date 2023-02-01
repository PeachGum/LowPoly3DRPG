using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HP_Stamina : MonoBehaviour
{
    public static Player_HP_Stamina instance;

    

    //최대 HP : 10
    //최대 스테미나 : 10
    private float decreaseStaminaValue = 0.5f, increaseStaminaValue = 2f, staminaTime = 0.01f, hp = 10;
    [NonSerialized]
    public float stamina = 10f, resetMonsterRange = 30f;

    private int maxHpIndex = 9, maxHp = 10, nowHpIndex = 9, maxStamina = 10;

    [NonSerialized]
    public bool isDecreasingStamina, isRecoveringStamina;

    public List<Image> hpImages;

    //public GameObject canvas;

    public Image staminaImage;
    public Coroutine RecoveryStaminaCoroutine, BlinkImageCoroutine;
    private Color staminaColor;
    private Color[] staminaColors = new Color[3];
    
    private WaitForSeconds waitForRecorveryStamina, staminaDuringTime, imageBlinkDuringTime;

    #region Singleton

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    #endregion
    void Start()
    {
        //staminaImage = canvas.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Image>();
        //빨강
        staminaColors[0] = new Color(0.8f,0,0,1);
        //주황
        staminaColors[1] = new Color(0.8f,0.5F,0,1);
        //초록
        staminaColors[2] = new Color(0, 1f, 0, 1f);

        //코루틴을 위한 WaitForSeconds 객체들을 캐싱
        waitForRecorveryStamina = new WaitForSeconds(2f);
        staminaDuringTime = new WaitForSeconds(staminaTime);
        imageBlinkDuringTime = new WaitForSeconds(1f);


    }

    void Update()
    {
        CheckStaminaImage();
    }
    
    public bool IncreaseHp(float val)
    {
        if (hp == 10)
        {
            return false;
        }
        while (hpImages[maxHpIndex].fillAmount <= 0.5f && val > 0)
        {
            if (hpImages[nowHpIndex].fillAmount <= 0.5f)
            {
                hpImages[nowHpIndex].fillAmount += 0.5f;
                hp += 0.5f;
                val -= 0.5f;
            }
            else
            {
                hpImages[++nowHpIndex].fillAmount += 0.5f;
                hp += 0.5f;
                val -= 0.5f;
            }
        }
        return true;

    }

    public void DecreaseHp(float val)
    {
        float tempVal = val;

        while (hpImages[0].fillAmount >= 0.5f && tempVal > 0)
        {
            if (hpImages[nowHpIndex].fillAmount >= 0.5f)
            {
                hpImages[nowHpIndex].fillAmount -= 0.5f;
                hp -= 0.5f;
                tempVal -= 0.5f;
            }
            else
            {
                hpImages[--nowHpIndex].fillAmount -= 0.5f;
                hp -= 0.5f;
                tempVal -= 0.5f;
            }

            if(hp <= 0)
            {
                hp = 0;
                SpawnScript.instance.spawnPoint = transform.position;
                Player_Equipment.instance.playerAttack.inventoryUI.dialogueUIManager.DieForUI();

                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, resetMonsterRange);

                //attackRange범위 안에 들어오는 객체들을 표시
                foreach (var col in hitEnemies)
                {
                    if (col.gameObject.CompareTag("MONSTER"))
                    {
                        Monster_Stat targetStat = col.GetComponent<Monster_Stat>();
                        if(targetStat != null)
                        {
                            targetStat.movingMonster.ResetMonster();
                        }
                    }
                }
            }
            else if(hp == maxHp)
            {
                return;
            }
            
        }

    }

    public void RefreshImage(Image img, float val)
    {
        img.fillAmount = val * 0.1f;
    }
    
    public bool IncreaseStamina(float val)
    {
        if (stamina == 10)
        {
            return false;
        }
        else if (maxStamina <= stamina + val)
        {
            stamina = maxStamina;
        }
        else
        {
            stamina += val;
        }
        RefreshImage(staminaImage, stamina);
        return true;

    }

    public void DecreaseStamina()
    {

        stamina -= decreaseStaminaValue * Time.deltaTime;
        RefreshImage(staminaImage, stamina);

        //스테미나 중복 회복 방지
        if (RecoveryStaminaCoroutine != null)
        {
            StopCoroutine(RecoveryStaminaCoroutine);
        }
        RecoveryStaminaCoroutine = StartCoroutine(RecoveryStamina());
        
        
    }
    public void DecreaseStamina(float val)
    {
        if(stamina >= val)
        {
            stamina -= val;
            RefreshImage(staminaImage, stamina);
        }

        if (RecoveryStaminaCoroutine != null)
        {
            StopCoroutine(RecoveryStaminaCoroutine);
        }
        RecoveryStaminaCoroutine = StartCoroutine(RecoveryStamina());
    }
    public IEnumerator RecoveryStamina()
    {
        yield return waitForRecorveryStamina;
        while (stamina < maxStamina)
        {
            IncreaseStamina(increaseStaminaValue * staminaTime);
            yield return staminaDuringTime;
        }
    }

    private void CheckStaminaImage()
    {
        var stFillAmount = staminaImage.fillAmount;
        if(stFillAmount <= 0.2f && stFillAmount > 0f && !staminaImage.color.Equals(staminaColors[0]))
        {
            staminaImage.color = staminaColors[0];
        }
        else if (stFillAmount <= 0.5f && stFillAmount > 0.2f && !staminaImage.color.Equals(staminaColors[1]))
        {
            staminaImage.color = staminaColors[1];
        }
        else if (stFillAmount > 0.5f && !staminaImage.color.Equals(staminaColors[2]))
        {
            staminaImage.color = staminaColors[2];
        }
    }
    
}
