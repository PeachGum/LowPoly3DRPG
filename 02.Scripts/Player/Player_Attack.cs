using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player_Attack : MonoBehaviour
{
    [HideInInspector]
    public int minAtk,maxAtk,specialAttackCount = 0;
    [HideInInspector]
    public float attackRange = 9f, specialAttackRange = 2.8f, arrowRange = 10f;
    [HideInInspector]
    public bool canAttack;

    private string bowCharging = "Player_BowCharging", bowShoot = "Player_BowShoot",
        axeSwing = "Player_AxeSwing", swordSlash = "Player_SwordSlash", axeHit = "Player_AxeHit", swordHit = "Player_SwordHit", swordSpecialAttackHit = "Player_SwordSpecialAttackHit";
    private string[] specialAttackSound = { "Player_AxeSpecialAttack1", "Player_AxeSpecialAttack2", "Player_AxeSpecialAttack3" };

    [HideInInspector]
    public Animator anim;
    private Transform attackPoint, specialAttackPoint, itemRoot, rangeSpawnPosition;
    public CameraManager cameraManager;

    //private float[] randomXYZ = new float[3];
    public InventoryUI inventoryUI;

    private MemoryPool arrowMemoryPool, swordSpecialEffectPool;
    public GameObject arrow, shootArrow, swordSpecialAttackEffect;
    public Item arrowItem;

    

    public bool isAttacking = false;
    //공격을 할 수 있을지 여부를 저장하는 변수

    [HideInInspector]
    public readonly int hashBowStartCharging = Animator.StringToHash("BowStartCharging"), 
        hashBowChargingDone = Animator.StringToHash("BowChargingDone"), 
        hashBowAttack = Animator.StringToHash("BowAttack"),
        hashAxeSpecialAttack = Animator.StringToHash("AxeSpecialAttack"),
        hashAxeAttack = Animator.StringToHash("AxeAttack"),
        hashSwordAttack = Animator.StringToHash("SwordAttack"),
        hashSwordSpecialAttack = Animator.StringToHash("SwordSpecialAttack");

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        arrowMemoryPool.pools.Clear();
        swordSpecialEffectPool.pools.Clear();
    }


    private void Awake()
    {
        attackPoint = transform.GetChild(2);
        specialAttackPoint = transform.GetChild(3);
        itemRoot = transform.GetChild(5);
        rangeSpawnPosition = transform.GetChild(4);

        
        arrowMemoryPool = new MemoryPool(shootArrow, rangeSpawnPosition);
        swordSpecialEffectPool = new MemoryPool(swordSpecialAttackEffect, specialAttackPoint);
        //백업
        //memoryPool = GetComponent<MemoryPool>();
        //memoryPool.SetMemoryPool(shootArrow, rangeSpawnPosition);

        anim = GetComponent<Animator>();
        inventoryUI = GameObject.Find("UIManager_Scripts").GetComponent<InventoryUI>();
        
    }

    void MeleeAttackStart()
    {
        isAttacking = true;
    }
    void MeleeAttackEnd()
    {
        isAttacking = false;
    }

    void BowChargingStart()
    {
        AudioManager.instance.SFXPlay(bowCharging);
    }

    void BowChargingDone()
    {
        if (anim.GetBool(hashBowStartCharging) && !isAttacking)
        {
            anim.SetBool(hashBowChargingDone, true);
        }
    }

    
    void OnAttack()
    {

        if(Player_Equipment.instance.weapon != null && canAttack)
        {
            //도끼 공격
            if (Player_Equipment.instance.weapon.itemCode == 5 && !isAttacking)
            {
                anim.SetTrigger(hashAxeAttack);
            }
            //검 공격
            if (Player_Equipment.instance.weapon.itemCode == 7 && !isAttacking)
            {
                anim.SetTrigger(hashSwordAttack);
            }
            //활 공격
            else if (Player_Equipment.instance.weapon.weaponType == WeaponItem.WeaponType.Bow)
            {
                int arrowItemIndex = Inventory.instance.FIndInventoryIndex(arrowItem);
                if (anim.GetBool(hashBowChargingDone) && !isAttacking && arrowItemIndex != -1)
                {
                    anim.SetTrigger(hashBowAttack);
                    //화살 하나 소모
                    Inventory.instance.onQuantityChangedCallback(arrowItem, arrowItemIndex, -1);
                    AudioManager.instance.SFXPlay(bowShoot);
                    Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                    Ray ray = Camera.main.ScreenPointToRay(screenCenter);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, arrowRange))
                    {
                        int index = arrowMemoryPool.CreatePool();
                        arrowMemoryPool.pools[index].GetComponent<ArrowScript>().SetTarget(hit.point);
                    }
                    //화살을 다 씀
                    if(Inventory.instance.FIndInventoryIndex(arrowItem) == -1)
                    {
                        EndZoom();
                    }
                }
                
            }
        }
    }

    void OnSpecialAttack()
    {
        
        if (Player_Equipment.instance.weapon != null && canAttack)
        {
            //도끼 특수공격
            if (Player_Equipment.instance.weapon.itemCode == 5 && !isAttacking)
            {
                if (Player_HP_Stamina.instance.stamina < 2f) return;
                specialAttackCount = 0;
                Player_HP_Stamina.instance.DecreaseStamina(2f);
                anim.SetTrigger(hashAxeSpecialAttack);

            }

            if (Player_Equipment.instance.weapon.itemCode == 7 && !isAttacking)
            {
                if (Player_HP_Stamina.instance.stamina < 3f) return;
                Player_HP_Stamina.instance.DecreaseStamina(2f);
                anim.SetTrigger(hashSwordSpecialAttack);
            }
            //활 줌
            if (Player_Equipment.instance.weapon.weaponType == WeaponItem.WeaponType.Bow)
            {
                //화살을 쏘고있지 않을떄 && 화살을 인벤토리에 소유하고 있을때
                if(!isAttacking && !anim.GetBool(hashBowStartCharging) && !anim.GetBool(hashBowChargingDone) && Inventory.instance.FIndInventoryIndex(arrowItem) != -1)
                {
                    Player_Equipment.instance.ChangeBow(false, true);
                    cameraManager.Zoom(true);
                    anim.SetBool(hashBowStartCharging, true);
                    arrow.SetActive(true);

                    //달리고 있을때 걷게 하기
                    if(Player_Equipment.instance.playerMovement.runCheck)
                    {
                        Player_Equipment.instance.playerMovement.runCheck = !Player_Equipment.instance.playerMovement.runCheck;
                        Player_Equipment.instance.playerMovement.anim.SetBool(Player_Equipment.instance.playerMovement.hashRun, Player_Equipment.instance.playerMovement.runCheck);
                    }
                        
                }
                else if(anim.GetBool(hashBowStartCharging) || anim.GetBool(hashBowStartCharging))
                {
                    EndZoom();
                }
            }
        }
    }

    public void EndZoom()
    {
        anim.SetBool(hashBowChargingDone, false);
        anim.SetBool(hashBowStartCharging, false);
        arrow.SetActive(false);
        Player_Equipment.instance.ChangeBow(true, false);
        cameraManager.Zoom(false);
        isAttacking = false;
    }


    void MeleeWeaponSFX()
    {
        //도끼 공격 사운드
        if(Player_Equipment.instance.weapon.itemCode == 5)
        {
            AudioManager.instance.SFXPlay(axeSwing);
        }
        //검 공격 사운드
        if (Player_Equipment.instance.weapon.itemCode == 7)
        {
            AudioManager.instance.SFXPlay(swordSlash);
        }

    }
    //플레이어의 공격 애니메이션 이벤트
    void AttackTime()
    {
        //attackPoint에서 attackRange의 범위만큼 구를 그려 범위 안에 있는 모든 콜라이더들 수집
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);

        //attackRange범위 안에 들어오는 객체들을 표시
        foreach (var col in hitEnemies)
        {
            if (col.gameObject.CompareTag("MONSTER"))
            {
                Monster_Stat targetStat = col.GetComponent<Monster_Stat>();
                if (targetStat.nowHp > 0)
                {
                    if(Player_Equipment.instance.weapon.itemCode == 5)
                    {
                        AudioManager.instance.SFXPlay(axeHit);
                    }
                    else if(Player_Equipment.instance.weapon.itemCode == 7)
                    {
                        AudioManager.instance.SFXPlay(swordHit);
                    }
                    
                    targetStat.SetHealth(-Random.Range(minAtk, maxAtk));
                    targetStat.movingMonster.target = transform;
                    //HitEffect(col.transform.position);
                    Player_Effect.instance.HitEffect(col.transform.position);
                }
            }
        }
    }
    void SpecialAttackTime()
    {
        //도끼 스페셜 공격 타입
        if(Player_Equipment.instance.weapon.itemCode == 5)
        {
            AudioManager.instance.SFXPlay(specialAttackSound[specialAttackCount++]);


            Player_Effect.instance.PlayEffect(Player_Effect.instance.swordSlashEffect);

            //attackPoint에서 attackRange의 범위만큼 구를 그려 범위 안에 있는 모든 콜라이더들 수집
            Collider[] hitEnemies = Physics.OverlapSphere(itemRoot.position, specialAttackRange);

            //attackRange범위 안에 들어오는 객체들을 표시
            foreach (var col in hitEnemies)
            {
                //플레이어일 경우 스킵

                if (col.gameObject.CompareTag("MONSTER"))
                {
                    Monster_Stat targetStat = col.GetComponent<Monster_Stat>();
                    if (targetStat.nowHp > 0)
                    {
                        targetStat.SetHealth(-Random.Range(minAtk, maxAtk));
                        targetStat.movingMonster.target = transform;
                        //HitEffect(col.transform.position);
                        Player_Effect.instance.HitEffect(col.transform.position);
                        AudioManager.instance.SFXPlay(axeHit);
                    }
                }
            }
            //공격도중 스테미나 회복 방지
            Player_HP_Stamina.instance.DecreaseStamina(0);
        }
        else if(Player_Equipment.instance.weapon.itemCode == 7)
        {
            AudioManager.instance.SFXPlay(swordSpecialAttackHit);
            int index = swordSpecialEffectPool.CreatePool();
        }
        
    }
}
