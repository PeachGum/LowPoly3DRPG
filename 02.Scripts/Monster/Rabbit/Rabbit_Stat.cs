using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit_Stat : Monster_Stat
{
    private Rabbit_Moving movingRabbit;
    public GameObject rabbitMeat;

    private void Awake()
    {
        Init();
        movingRabbit = GetComponent<Rabbit_Moving>();
        //base.movingMonster = movingRabbit;
        dropItem = rabbitMeat;
    }

    void OnEnable()
    {
        RespawnMonster();
        if (movingRabbit != null)
        {
            movingRabbit.nav.speed = 4;
            movingRabbit.ChangeRunSpeed(1.0f);
        }
        
    }
    public override void SetHealth(int health)
    {
        //몬스터의 체력이 처음에는 가려져있다가 피격시에 체력바가 보임
        if (!slider.gameObject.activeSelf)
        {
            movingRabbit.nav.speed = 8;
            movingRabbit.ChangeRunSpeed(2.0f);
            slider.gameObject.SetActive(true);
        }

        base.SetHealth(health);

        //한대 맞으면 속도가 빨라짐

    }
}
