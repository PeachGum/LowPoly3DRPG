using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class Cow_Moving : Monster_Moving
{
    int randomInt;
    public readonly int hashRunning = Animator.StringToHash("isRunning");
    public readonly int hashEating = Animator.StringToHash("Eating");

    void Awake()
    {
        base.Init();
    }
    private void OnEnable()
    {
        nav.speed = 1f;
        nav.isStopped = false;
        isRandomAnimation = true;
        StartCoroutine(RandomAnimation1());
    }
    void FixedUpdate()
    {
        if(!isRandomAnimation)
        {
            ChangeDestination();
        }

        //목적지에 도달하고 제자리 걸음 방지용
        if(anim.GetBool(hashWalking) && nav.velocity.sqrMagnitude == 0f && nav.remainingDistance <= 0.1f)
        {
            anim.SetBool(hashIdle, true);
            anim.SetBool(hashWalking, false);
        }
    }
    public override void RandomAnimation()
    {
        randomInt = UnityEngine.Random.Range(1, 4);
        //경로를 가지고 있지 않을때
        if (nav.velocity.sqrMagnitude == 0f)
        {
            //랜덤으로 0,1,2 int를 추출하고 1은 idle, 2은 walk, 3는 eat로 애니메이션을 랜덤으로 설정
            
            //Idle 상태
            if (randomInt == 1)
            {
                anim.SetBool(hashWalking, false);
                anim.SetBool(hashIdle, true);
            }
            if (randomInt == 2)
            {
                anim.SetBool(hashWalking, true);
                anim.SetBool(hashIdle, false);
                ChangeDestination();
            }
            if (randomInt == 3)
            {
                anim.SetBool(hashWalking, false);
                anim.SetBool(hashIdle, false);
                anim.SetTrigger(hashEating);
            }
        }
    }

    public IEnumerator RandomAnimation1()
    {
        while(isRandomAnimation)
        {
            randomWaitTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(randomWaitTime);
            RandomAnimation();
        }

    }

   
}
