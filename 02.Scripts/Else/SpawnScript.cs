using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnScript : MonoBehaviour
{
    #region Singleton
    public static SpawnScript instance;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    #endregion
    Scene scene;
    public Vector3 spawnPoint = new Vector3(0,0,0);
    public GameObject Player;
    public GameObject Camera;


    public void Start()
    {
        SetPosition();
        scene = SceneManager.GetActiveScene();
        if(scene.name != "Player")
        {
            AudioManager.instance.BGMPlay(scene.name);
        }
        
    }

    public void SetPosition()
    {
        Player.transform.position = spawnPoint;
        Camera.transform.position = spawnPoint;
    }
}
