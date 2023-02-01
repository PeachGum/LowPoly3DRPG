using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_Moving : Monster_Moving
{
    [HideInInspector]
    public Monster_Attack snakeAttack;


    //private SphereCollider detectionCollider;

    void Awake()
    {

        snakeAttack = transform.GetChild(1).GetComponent<Monster_Attack>();


        base.Init();
    }
    private void Start()
    {
        target = GameObject.Find("_PLAYER").transform;
        //target = Player_HP_Stamina.instance.transform;

    }
    private void OnEnable()
    {
        ResetMonster();

    }
    void Update()
    {
        if (!isRandomAnimation)
        {
            DetectionPlayer();
            //ChangeDestination();
        }

        //�������� �����ϰ� ���ڸ� ���� ������
        //if (!snakeAttack.isAttacking && anim.GetBool(hashWalking) && nav.velocity.sqrMagnitude == 0f && nav.remainingDistance <= 0.1f)        
        if (!snakeAttack.isAttacking && anim.GetBool(hashWalking) && nav.velocity.sqrMagnitude == 0f && nav.remainingDistance <= 0.1f)
        {
            anim.SetBool(hashIdle, true);
            anim.SetBool(hashWalking, false);
        }
    }
    public override void RandomAnimation()
    {
        int randomInt = Random.Range(1, 3);
        //��θ� ������ ���� ������
        if (nav.velocity.sqrMagnitude == 0f && isRandomAnimation)
        {
            //�������� 0,1,2 int�� �����ϰ� 1�� idle, 2�� walk, 3�� eat�� �ִϸ��̼��� �������� ����

            //Idle ����
            if (randomInt == 1)
            {
                anim.SetBool(hashIdle, true);
                anim.SetBool(hashWalking, false);
                anim.SetBool(hashAttack, false);
            }
            //��θ� �����Ͽ� �ȱ�
            if (randomInt == 2)
            {
                anim.SetBool(hashWalking, true);
                anim.SetBool(hashIdle, false);
                anim.SetBool(hashAttack, false);
                ChangeDestination();
            }
        }



    }



    //�÷��̾ ���� ���� ���������� �÷��̾ �߰���.
    //�ڽ� ������Ʈ�� Spider_Detection ��ũ��Ʈ�� ȣ��
    public void DetectionPlayer()
    {

        isRandomAnimation = false;
        StopCoroutine(RandomAnimation_Couroutine());

        nav.speed = 5f;
        nav.SetDestination(target.position);
        if (!anim.GetBool(hashWalking))
        {
            anim.SetBool(hashWalking, true);
        }
        anim.SetBool(hashIdle, false);
    }
    public void AttackStart()
    {

        snakeAttack.AttackStart();
    }
    public void Attacking()
    {
        snakeAttack.Attacking();
    }
    public void AttackEnd()
    {
        snakeAttack.AttackEnd();
    }

}
