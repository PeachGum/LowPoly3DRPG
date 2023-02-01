using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_Stat : Monster_Stat
{
    void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        RespawnMonster();
    }
    public override void SetHealth(int health)
    {
        //������ ü���� ó������ �������ִٰ� �ǰݽÿ� ü�¹ٰ� ����
        if (!slider.gameObject.activeSelf && nowHp > 0)
        {
            movingMonster.isRandomAnimation = false;
            StopCoroutine(movingMonster.RandomAnimation_Couroutine());
            slider.gameObject.SetActive(true);
        }

        base.SetHealth(health);


        //�Ѵ� ������ �ӵ��� ������


    }
}
