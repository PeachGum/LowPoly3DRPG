using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class UIObjectControl : MonoBehaviour
{
    private float lookFloat = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 24 && transform.position.x <= -126f)
        {
            StartCoroutine(nameof(LookTransform));
            transform.rotation = Quaternion.Euler(0, lookFloat, 0);
            
            transform.Translate(Vector3.right * 12.0f * Time.deltaTime);
        }
        

        else
        {   
            transform.rotation = Quaternion.Euler(0, lookFloat, 0);
            
        }
    }

    IEnumerator LookTransform()
    {
        lookFloat += 180;
        
        yield break;
        
    }
}
