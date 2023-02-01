using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawn : MonoBehaviour
{
    //public List<GameObject> monsters;
    public List<Transform> randomSpwanPosition;
    private WaitForSeconds waitForRespawn;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RespawnMonster(GameObject target)
    {
        StartCoroutine(SpwanMonster(target));
    }

    IEnumerator SpwanMonster(GameObject target)
    {
        waitForRespawn = new WaitForSeconds(Random.Range(5.0f, 10f));
        yield return waitForRespawn;
        target.transform.position = randomSpwanPosition[Random.Range(0, randomSpwanPosition.Count)].position;
        target.SetActive(true);
    }
}
