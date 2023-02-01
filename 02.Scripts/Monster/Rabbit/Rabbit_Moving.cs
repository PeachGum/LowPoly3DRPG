using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Rabbit_Moving : Monster_Moving
{
    private readonly int hashRunning = Animator.StringToHash("isRunning");
    private readonly int hashRunSpeed = Animator.StringToHash("RunSpeed");

    void Awake()
    {
        Init();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        
    }

    void FixedUpdate()
    {
        ChangeDestination();
    }
    void OnEnable()
    {
        anim.SetBool(hashRunning,true);
    }

    public void ChangeRunSpeed(float val)
    {
        anim.SetFloat(hashRunSpeed, val);
    }
    // Start is called before the first frame update
}