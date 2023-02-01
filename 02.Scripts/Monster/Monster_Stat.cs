using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster_Stat : MonoBehaviour
{

    //slider.value 가 내 현재 체력
    public int maxHp;
    [HideInInspector]
    public int nowHp; 
    public float atk;
    [HideInInspector]
    public bool isDie = false;

    protected string dieSound;

    public GameObject dropItem;
    protected Color baseColor , hitColor = new Color(1, 0, 0);


    [HideInInspector]
    public Slider slider;
    [HideInInspector]
    public TextMeshProUGUI hpText;
    
    [HideInInspector]
    public Monster_Moving movingMonster;

    [HideInInspector]
    public MonsterSpawn monsterSpawn;

    public SkinnedMeshRenderer skinnedMeshRenderer;
    protected readonly int hashDie = Animator.StringToHash("Die");
    protected MemoryPoolForMap memoryPool;
    WaitForSeconds waitForDisable = new WaitForSeconds(7f);

    


    private void OnEnable()
    {
        isDie = false;
        RespawnMonster();
    }
    private void Start()
    {
        if (skinnedMeshRenderer != null)
        {
            baseColor = skinnedMeshRenderer.material.color;
        }
        memoryPool = GameObject.Find("MapManager").GetComponent<MemoryPoolForMap>();

    }
    public void Init()
    {
        monsterSpawn = FindObjectOfType<MonsterSpawn>();
        movingMonster = GetComponent<Monster_Moving>();
        slider = transform.GetChild(0).GetChild(0).GetComponentInChildren<Slider>();
        hpText = transform.GetChild(0).GetChild(0).GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        nowHp = maxHp;
    }

    public void RespawnMonster()
    {
        if(slider != null)
        {
            slider.gameObject.SetActive(true);
            SetMaxHealth(maxHp);
            slider.gameObject.SetActive(false);
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        nowHp = health;
        hpText.text = health.ToString();
    }

    public virtual void SetHealth(int health)
    {
        if (nowHp > 0)
        {
            nowHp += health;
            slider.value = nowHp;
            hpText.text = nowHp.ToString();
            if(health < 0)
            {
                skinnedMeshRenderer.material.color = hitColor;
                //데미지 글짜 표시
                if(memoryPool != null)
                {
                    memoryPool.CreatePoolForText(transform, (health * -1).ToString());
                }
                Invoke("RecoveryColor", 0.2f);
            }
            
        }
        if (nowHp <= 0 && slider.gameObject.activeSelf)
        {
            Die();
        }
    }

    public void Die()
    {
        isDie = true;
        DieSound();
        DropItem();

        slider.gameObject.SetActive(false);
        movingMonster.anim.SetTrigger(hashDie);
        movingMonster.nav.speed = 0;
        movingMonster.nav.isStopped = true;
        StartCoroutine(DisableMonster());
    }
    protected IEnumerator DisableMonster()
    {
        yield return waitForDisable;
        gameObject.SetActive(false);
        if(monsterSpawn != null)
        {
            monsterSpawn.RespawnMonster(gameObject);
        }
        
    }
    public void DieSound()
    {
        if(dieSound != null && dieSound != "")
        {
            AudioManager.instance.SFXPlay(dieSound);
        }
    }
    public void DropItem()
    {
        if(dropItem != null)
        {
            Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z); 
            Instantiate(dropItem, dropPosition, Quaternion.identity);
        }
    }

    public void RecoveryColor()
    {
        skinnedMeshRenderer.material.color = baseColor;
    }
    //public void MemoryPoolCreate(GameObject obj)
    //{
    //    for(int i=0; i<memoryPool.Count; i++)
    //    {
    //        if (memoryPool[i].)
    //        {
    //            memoryPool[i] = obj;
    //        }
    //    }
    //}
}
