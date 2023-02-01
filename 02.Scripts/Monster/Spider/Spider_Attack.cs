using UnityEngine;

public class Spider_Attack : Monster_Attack
{ 
    public override void Attacking()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange);

        foreach (var targets in hitEnemies)
        {
            //�÷��̾��� ��� ��ŵ

            if (targets.gameObject.CompareTag("PLAYER"))
            {
                Player_HP_Stamina.instance.DecreaseHp(movingMonster.monsterStat.atk);
                // 1 2 3 4 �����߿� 4�� ������ �� ������ �� ��, 25%
                int randomInt = Random.Range(1, 5);
                if(randomInt.Equals(4))
                {
                    StartCoroutine(Player_Effect.instance.PoisonAttacked());
                }
                
            }
            else
            {
                movingMonster.anim.SetBool(movingMonster.hashWalking, true);
                movingMonster.anim.SetBool(movingMonster.hashAttack, false);
            }
        }
    }
}
