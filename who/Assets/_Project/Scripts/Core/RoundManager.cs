using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager: MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Rounds")]
    public rounds currentRound = rounds.round1;

    [Header("References")]
    [SerializeField] TimerManager timerManager;

    [SerializeField] List<RoundScene> roundScenes;

    public event Action OnAllRoundsCompleted;

    private bool roundEnding;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    private void Start()
    {
        timerManager.OnTimerFinished += EndRound;
    }

    public void StartFirstRound()
    {
        currentRound = rounds.round1;
        StartCoroutine(LoadFirstScene());
    }

    void EndRound()
    {
        if (roundEnding) return;
        roundEnding = true;

        StartCoroutine(RoundTransition());
    }

    IEnumerator RoundTransition()
    {
        yield return ScreenFader.instance.FadeOut();

        currentRound++;

        if (currentRound > rounds.round3)
        {
            OnAllRoundsCompleted?.Invoke();
            yield break;
        }

        GameManager.instance.RegisterDeath();

        var scene = roundScenes.Find(r => r.round == currentRound);

        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.sceneName);

        roundEnding = false;

        yield return ScreenFader.instance.FadeIn();
    }

    IEnumerator LoadFirstScene()
    {
        yield return ScreenFader.instance.FadeOut();

        var scene = roundScenes.Find(r => r.round == currentRound);
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.sceneName);

        roundEnding = false;

        yield return ScreenFader.instance.FadeIn();
    }
}
