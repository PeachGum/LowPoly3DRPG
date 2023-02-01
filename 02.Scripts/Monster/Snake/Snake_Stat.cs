using UnityEngine;

public class Snake_Stat : Monster_Stat
{
    void Awake()
    {
        Init();
    }

    //private void OnEnable()
    //{
    //    RespawnMonster();
    //}
    public override void SetHealth(int health)
    {
        //몬스터의 체력이 처음에는 가려져있다가 피격시에 체력바가 보임
        if (!slider.gameObject.activeSelf && nowHp > 0)
        {
            movingMonster.isRandomAnimation = false;
            StopCoroutine(movingMonster.RandomAnimation_Couroutine());
            slider.gameObject.SetActive(true);
        }

        base.SetHealth(health);

        //한대 맞으면 속도가 빨라짐


    }
}
