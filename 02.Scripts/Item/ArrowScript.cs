using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public Vector3 target;
    public float speed;
    private string arrowHit = "Arrow_ArrowHit";
    Rigidbody rigid;

    private void Update()
    {
        if(target != null)
        {
            
            float speed = this.speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
        }
    }

    public void SetTarget(Vector3 pos)
    {
        target = pos;
        transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("MONSTER"))
        {
            AudioManager.instance.SFXPlay(arrowHit);
            Monster_Stat targetMonster = col.transform.GetComponent<Monster_Stat>();
            targetMonster.SetHealth(-Random.Range(Player_Equipment.instance.playerAttack.minAtk, Player_Equipment.instance.playerAttack.maxAtk));
            targetMonster.movingMonster.target = Player_Equipment.instance.player;
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(WaitForActive(false, 5f));
        }
    }

    IEnumerator WaitForActive(bool b, float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(b);
    }
}
