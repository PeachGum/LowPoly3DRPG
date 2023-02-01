using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Respawn : MonoBehaviour
{
    public CameraManager cameraManager;
    // Start is called before the first frame update
    public void RespawnAtVillage()
    {
        SpawnScript.instance.spawnPoint = new Vector3(0,0,0);
        SpawnScript.instance.SetPosition();
        SceneManager.LoadScene("Village");

        Respawn();

    }

    public void RespawnAtDiePosition()
    {
        Respawn();
    }

    void Respawn()
    {
        Player_Equipment.instance.playerMovement.Respawn();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Player_HP_Stamina.instance.IncreaseHp(10);
        Player_HP_Stamina.instance.IncreaseStamina(10);
        Player_Equipment.instance.playerMovement.ui.OffAllUI();
        cameraManager.StopCamera(false);
    }
}
