using UnityEngine;
using Random = UnityEngine.Random;

public class Spider_Moving : Monster_Moving
{
    [HideInInspector]
    public Monster_Attack spiderAttack;
    public readonly int hashScared = Animator.StringToHash("isScared");
    void Awake()
    {
        
        spiderAttack = transform.GetChild(1).GetComponent<Monster_Attack>();

        base.Init();
    }
    private void Start()
    {
        target = GameObject.Find("_PLAYER").transform;

    }
    private void OnEnable()
    {
        ResetMonster();
    }
    void FixedUpdate()
    {
        if (!isRandomAnimation)
        {
            DetectionPlayer();
        }

        //�������� �����ϰ� ���ڸ� ���� ������
        if (!spiderAttack.isAttacking && anim.GetBool(hashWalking) && nav.velocity.sqrMagnitude == 0f && nav.remainingDistance <= 0.1f)
        {
            anim.SetBool(hashIdle, true);
            anim.SetBool(hashWalking, false);
        }
    }
    public override void RandomAnimation()
    {
        int randomInt = Random.Range(1, 4);
        //��θ� ������ ���� ������
        if (nav.velocity.sqrMagnitude == 0f && isRandomAnimation)
        {
            //�������� 0,1,2 int�� �����ϰ� 1�� idle, 2�� walk, 3�� eat�� �ִϸ��̼��� �������� ����

            //Idle ����
            if (randomInt == 1)
            {
                anim.SetBool(hashIdle, true);
                anim.SetBool(hashWalking, false);
                anim.SetBool(hashScared, false);
                anim.SetBool(hashAttack, false);
            }
            //��θ� �����Ͽ� �ȱ�
            if (randomInt == 2)
            {
                anim.SetBool(hashWalking, true);
                anim.SetBool(hashIdle, false);
                anim.SetBool(hashScared, false);
                anim.SetBool(hashAttack, false);
                ChangeDestination();
            }
            //������ ���
            if (randomInt == 3)
            {
                anim.SetBool(hashScared, true);
                anim.SetBool(hashWalking, false);
                anim.SetBool(hashIdle, false);
                anim.SetBool(hashAttack, false);
            }
        }



    }

    
    
    //�÷��̾ ���� ���� ���������� �÷��̾ �߰���.
    //�ڽ� ������Ʈ�� Spider_Detection ��ũ��Ʈ�� ȣ��
    public void DetectionPlayer()
    {

            isRandomAnimation = false;
            StopCoroutine(RandomAnimation_Couroutine());

            nav.speed = 4f;
            nav.SetDestination(target.position);
            if (!anim.GetBool(hashWalking))
            {
                anim.SetBool(hashWalking, true);
            }
            anim.SetBool(hashIdle, false);
            anim.SetBool(hashScared, false);
    }
    public void AttackStart()
    {

        spiderAttack.AttackStart();
    }
    public void Attacking()
    {
        spiderAttack.Attacking();
    }
    public void AttackEnd()
    { 
        spiderAttack.AttackEnd();
    }


}
