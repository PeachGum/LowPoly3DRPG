using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Detection : MonoBehaviour
{
    private Monster_Moving movingSpider;
    // Start is called before the first frame update
    void Awake()
    {
        movingSpider = transform.GetComponentInParent<Monster_Moving>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            if (movingSpider != null && movingSpider.target == null)
            {
                movingSpider.target = col.transform;
            }
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            if(movingSpider != null)
            {
                if (movingSpider.isRandomAnimation)
                {
                    movingSpider.isRandomAnimation = false;
                }
            }
        }
    }

    //private void OnTriggerExit(Collider col)
    //{
    //    if (col.CompareTag("PLAYER"))
    //    {
    //        movingSpider.isRandomAnimation = true;
    //    }
    //}
    
}
