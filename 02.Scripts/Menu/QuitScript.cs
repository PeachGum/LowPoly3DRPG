using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitScript : MonoBehaviour
{
    public GameObject quitWindow;
    public List<GameObject> quitHide;

    public void OnQuitWindow()
    {
        AudioManager.instance.SFXPlay("ButtonClick");
        quitWindow.SetActive(true);
        if (quitHide != null)
        {
            foreach (var obj in quitHide)
            {
                obj.SetActive(false);
            }
        }
    }

    public void Quit()
    {
        AudioManager.instance.SFXPlay("ButtonClick");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void CancelQuit()
    {
        AudioManager.instance.SFXPlay("ButtonClick");
        quitWindow.SetActive(false);

        if (quitHide != null)
        {
            foreach (var obj in quitHide)
            {
                obj.SetActive(true);
            }
        }
    }
}
