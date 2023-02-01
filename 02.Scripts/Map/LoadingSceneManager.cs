using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string loadScene = null;

    [SerializeField]
    private static GameObject player;

    [SerializeField] Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }


    public static void LoadScene(string firstSceneName)
    {

        loadScene = firstSceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadScene);

        op.allowSceneActivation = false;
        //else
        //{
        //    op.allowSceneActivation = false;
        //}

        loadScene = null;

        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.8f, 1f, timer);
                if (progressBar.fillAmount >= 1.0f)
                {
                    if(player != null)
                    {
                        player.SetActive(true);
                        player = null;
                    }

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    public static void PlayerObjectPush(GameObject val)
    {
        player = val;
    }
}
