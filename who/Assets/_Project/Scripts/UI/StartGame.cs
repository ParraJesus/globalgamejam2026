using System.Collections;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void GameStart()
    {
        StartCoroutine(LoadStarGame());
    }

    IEnumerator LoadStarGame()
    {
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("InitilialScene");
    }
}
