using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public rounds CurrentRound { get; private set; } = rounds.round1;
    public int CurrentAttemptDialogue { get; private set; } = 5;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void NextRound()
    {
        if (CurrentRound < rounds.round3)
            CurrentRound++;

        if (CurrentAttemptDialogue > 0)
            CurrentAttemptDialogue--;
    }
}
