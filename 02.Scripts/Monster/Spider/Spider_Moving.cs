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

        //목적지에 도달하고 제자리 걸음 방지용
        if (!spiderAttack.isAttacking && anim.GetBool(hashWalking) && nav.velocity.sqrMagnitude == 0f && nav.remainingDistance <= 0.1f)
        {
            anim.SetBool(hashIdle, true);
            anim.SetBool(hashWalking, false);
        }
    }
    public override void RandomAnimation()
    {
        int randomInt = Random.Range(1, 4);
        //경로를 가지고 있지 않을때
        if (nav.velocity.sqrMagnitude == 0f && isRandomAnimation)
        {
            //랜덤으로 0,1,2 int를 추출하고 1은 idle, 2은 walk, 3는 eat로 애니메이션을 랜덤으로 설정

            //Idle 상태
            if (randomInt == 1)
            {
                anim.SetBool(hashIdle, true);
                anim.SetBool(hashWalking, false);
                anim.SetBool(hashScared, false);
                anim.SetBool(hashAttack, false);
            }
            //경로를 지정하여 걷기
            if (randomInt == 2)
            {
                anim.SetBool(hashWalking, true);
                anim.SetBool(hashIdle, false);
                anim.SetBool(hashScared, false);
                anim.SetBool(hashAttack, false);
                ChangeDestination();
            }
            //기지개 펴기
            if (randomInt == 3)
            {
                anim.SetBool(hashScared, true);
                anim.SetBool(hashWalking, false);
                anim.SetBool(hashIdle, false);
                anim.SetBool(hashAttack, false);
            }
        }



    }

    
    
    //플레이어가 범위 내에 접근했을때 플레이어를 추격함.
    //자식 오브젝트에 Spider_Detection 스크립트로 호출
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
