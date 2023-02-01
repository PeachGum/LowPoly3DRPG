using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarLookCamera : MonoBehaviour
{
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}
