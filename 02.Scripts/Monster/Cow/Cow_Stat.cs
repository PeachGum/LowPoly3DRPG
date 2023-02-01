using UnityEngine;

public class Cow_Stat : Monster_Stat
{
    [HideInInspector]
    private Cow_Moving movingCow;
    private string cowCry = "Cow_Cry", cowDead = "Cow_Dead";

    void Awake()
    {
        Init();
        movingCow = GetComponent<Cow_Moving>();
        dieSound = cowDead;


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
            StopCoroutine(movingCow.RandomAnimation1());
            movingCow.nav.speed = 7;
            movingCow.anim.SetTrigger(movingCow.hashRunning);
            movingCow.isRandomAnimation = false;
            slider.gameObject.SetActive(true);
            AudioManager.instance.SFXPlay(cowCry);
        }


        base.SetHealth(health);

        //한대 맞으면 속도가 빨라짐
        
        
    }


}
