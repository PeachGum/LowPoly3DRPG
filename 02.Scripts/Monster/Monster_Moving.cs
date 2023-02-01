using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Monster_Moving : MonoBehaviour
{
    [HideInInspector]
    public Monster_Stat monsterStat;
    [HideInInspector]
    public Animator anim;
    private NavMeshHit hit;
    [HideInInspector]
    public NavMeshAgent nav;


    public Transform target;
    [HideInInspector]
    public bool isRandomAnimation = true;

    public readonly int hashWalking = Animator.StringToHash("isWalking");
    public readonly int hashIdle = Animator.StringToHash("isIdle");
    public readonly int hashAttack = Animator.StringToHash("isAttack");

    protected float randomWaitTime, walkRadius = 70, idleSpeed;

    private Vector3 randomDirection, finalPosition, destination;

    public void Init()
    {
        monsterStat = GetComponent<Monster_Stat>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        idleSpeed = nav.speed;
    }

    
    public Vector3 RandomNavmeshLocation(float radius) {
        randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }
    
    public virtual void ChangeDestination()
    {

        if (nav.pathStatus == NavMeshPathStatus.PathComplete && nav.remainingDistance <= nav.stoppingDistance)
        {
            destination = RandomNavmeshLocation(walkRadius);
            nav.SetDestination(destination);
        }
    }

    public virtual void RandomAnimation() { }

    public IEnumerator RandomAnimation_Couroutine()
    {
        while (isRandomAnimation)
        {
            randomWaitTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(randomWaitTime);
            RandomAnimation();
        }
    }

    public virtual void ResetMonster()
    {
        nav.speed = idleSpeed;
        nav.isStopped = false;
        nav.SetDestination(transform.position);
        anim.SetBool(hashIdle, true);
        isRandomAnimation = true;
        target = null;
        StartCoroutine(RandomAnimation_Couroutine());
    }
}
