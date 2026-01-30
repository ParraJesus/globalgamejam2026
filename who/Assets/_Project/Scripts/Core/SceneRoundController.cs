using UnityEngine;

public class SceneRoundController: MonoBehaviour
{
    public rounds sceneRound;

    private void Start()
    {
        GameManager.instance.SetCurrentRound(sceneRound);
        TimerManager.instance.ResetTimer();
        TimerManager.instance.SetIsRunning(true);

        Debug.Log($"Escena de ronda {sceneRound}");
    }
}
