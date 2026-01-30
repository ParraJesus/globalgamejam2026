using System;
using System.Collections;
using UnityEngine;

public class RoundManager: MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Rounds")]
    public rounds currentRound = rounds.round1;

    [Header("References")]
    [SerializeField] TimerManager timerManager;

    public event Action OnAllRoundsCompleted;

    private bool roundEnding;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        timerManager.OnTimerFinished += EndRound;
    }

    public void StartFirstRound()
    {
        ScreenFader.instance.FadeOut();
        currentRound = rounds.round1;
        ScreenFader.instance.FadeIn();
        StartRound();
    }

    void StartRound()
    {
        roundEnding = false;
        timerManager.ResetTimer();

        GameManager.instance.SetCurrentRound(currentRound);
        Debug.Log($"Ronda iniciada: {currentRound}");

        ResetNPCs();
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
            Debug.Log("Muere el detective");
            OnAllRoundsCompleted?.Invoke();
            yield break;
        }

        GameManager.instance.SetCurrentRound(currentRound);

        yield return ScreenFader.instance.FadeIn();

        StartRound();
    }

    void ResetNPCs()
    {
        var npcs = FindObjectsByType<NpcController>(sortMode: FindObjectsSortMode.InstanceID);
        foreach (var npc in npcs)
        {
            npc.ForceReset();
        }
    }

}
