using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    private Player_UICollider playerUi;

    public Vector3 spawnPoint;
    //포탈 정보 StringBuilder타입의 List
    public string portalInfo;

    private void Start()
    {
        //"[E] <color=green>광활한 초원</color>으로 이동"
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            playerUi = col.GetComponent<Player_UICollider>();
            playerUi.ui.listUI[2].GetComponentInChildren<TextMeshProUGUI>().text = portalInfo;
            playerUi.ui.OnUI(2);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("PLAYER"))
        {
            playerUi.ui.OffUI(2);
        }
    }
    
    public void MoveScene()
    {
        SpawnScript.instance.spawnPoint = spawnPoint;
        LoadingSceneManager.LoadScene(transform.name);
        //SceneManager.LoadScene(transform.name);
        AudioManager.instance.BGMPlay(transform.name);

        //독에 걸려있다면 멈춤
        Player_Effect.instance.StopPoison();
    }
}
