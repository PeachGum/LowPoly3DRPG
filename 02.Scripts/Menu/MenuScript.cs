using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Scene Village;
    public SettingScript settingScript;
    public GameObject player;

    private void Start()
    {
        AudioManager.instance.BGMPlay("Forest");

        //SceneManager.LoadScene("Player", LoadSceneMode.Single);
        //SceneManager.LoadScene("BossStage", LoadSceneMode.Additive);
    }
    public void OnClickStart()
    {
        StartCoroutine(StartCoroutine());
    }

    IEnumerator StartCoroutine()
    {
        AudioManager.instance.SFXPlay("ButtonClick");
        settingScript.PullSettingValues();
        yield return new WaitForSeconds(0.2f);
        LoadingSceneManager.LoadScene("Village");
        LoadingSceneManager.PlayerObjectPush(player);
        AudioManager.instance.BGMPlay("Village");
    }
}
