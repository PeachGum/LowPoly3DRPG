using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            //DetectionPlayer(col.transform);
        }
    }
}
