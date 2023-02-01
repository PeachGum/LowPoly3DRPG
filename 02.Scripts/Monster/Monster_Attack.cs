using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Attack : MonoBehaviour
{
    //�ʼ� ��� ����
    //���� ���ο� Canvas�� 1��°, ���� ������ 2��°, ���� ������ 3��°

    protected Monster_Moving movingMonster;
    protected Transform attackPoint;
    public float attackRange = 5f;
    [HideInInspector]
    public bool isAttacking = false;

    public string attackSound;
    // Start is called before the first frame update
    void Awake()
    {
        attackPoint = transform.parent.GetChild(2);
        movingMonster = transform.GetComponentInParent<Monster_Moving>();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            if (movingMonster != null && !movingMonster.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            //if (movingMonster != null)
            {
                Attack();
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            if (movingMonster != null)
            {

                
            }
        }
    }
   
    public void Attack()
    {
        //isAttacking = true;
        movingMonster.anim.SetBool(movingMonster.hashAttack, true);
        movingMonster.anim.SetBool(movingMonster.hashWalking, false);

    }


    public void StopAttack()
    {

        isAttacking = false;
        movingMonster.anim.SetBool(movingMonster.hashAttack, false);
        movingMonster.anim.SetBool(movingMonster.hashWalking, true);



    }
    public void AttackStart()
    {
        if (attackSound != "")
        {
            AudioManager.instance.SFXPlay(attackSound);
        }
        isAttacking = true;

    }
    public virtual void Attacking()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange);

        foreach (var targets in hitEnemies)
        {
            //�÷��̾��� ��� ��ŵ

            if (targets.gameObject.CompareTag("PLAYER"))
            {
                Player_HP_Stamina.instance.DecreaseHp(movingMonster.monsterStat.atk);
            }
            else
            {
                movingMonster.anim.SetBool(movingMonster.hashWalking, true);
                movingMonster.anim.SetBool(movingMonster.hashAttack, false);
            }
        }
    }
    public void AttackEnd()
    {

        isAttacking = false;


    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(attackPoint.transform.position, attackRange);
    //}
}
