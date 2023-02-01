using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Position : MonoBehaviour
{
    ParticleSystem self;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<ParticleSystem>();
        player = transform.parent.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //����Ʈ�� �������϶� �÷��̾�� ����ö�
        if(self != null && player != null && self.isPlaying)
        {
            transform.position = player.position;
        }
    }
}
