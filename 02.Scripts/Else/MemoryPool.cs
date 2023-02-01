using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryPool
{
    [SerializeField]
    private GameObject obj;

    //[HideInInspector]
    public List<GameObject> pools = new List<GameObject>();
    //private List<TextMeshProUGUI> textPools = new List<TextMeshProUGUI>();

    private Transform spawn;

    public MemoryPool(GameObject obj, Transform spawn)
    {
        this.obj = obj;
        this.spawn = spawn;

    }
    public void SetMemoryPool(GameObject obj, Transform spawn)
    {
        this.obj = obj;
        this.spawn = spawn;
        
    }
    public void SetMemoryPool(GameObject obj)
    {
        this.obj = obj;
    }


    // Start is called before the first frame update
    public int CreatePool()
    {
        if (pools.Count == 0)
        {
            MemoryExpansion();
        }
        for (int i = 0; i < pools.Count; i++)
        {
            if (!pools[i].activeSelf)
            {
                pools[i].transform.position = spawn.transform.position;
                pools[i].transform.rotation = spawn.transform.rotation;
                pools[i].SetActive(true);
                return i;
            }
        }
        //메모리풀 공간이 없을때 확장
        MemoryExpansion();
        return 0;
    }

    public int CreatePoolForText(Transform target, string st)
    {
        if (pools.Count == 0)
        {
            MemoryExpansion();
        }
        for (int i = 0; i < pools.Count; i++)
        {
            if (!pools[i].activeSelf)
            {
                pools[i].transform.position = target.transform.position;
                //pools[i].transform.rotation = target.transform.rotation;
                pools[i].SetActive(true);

                if(pools[i].GetComponent<TextMeshPro>() != null)
                {
                    pools[i].GetComponent<TextMeshPro>().text = st;
                }
                return i;
            }
        }
        //메모리풀 공간이 없을때 확장
        MemoryExpansion();
        return 0;
    }


    void MemoryExpansion()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject mpArrow = GameObject.Instantiate(obj) as GameObject;//Instantiate<GameObject>(obj);
            pools.Add(mpArrow);
            mpArrow.SetActive(false);
        }
    }


}
