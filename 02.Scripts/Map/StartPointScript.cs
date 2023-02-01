using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPointScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SpawnScript.instance != null)
        {
            SpawnScript.instance.SetPosition();
        }
    }
}
