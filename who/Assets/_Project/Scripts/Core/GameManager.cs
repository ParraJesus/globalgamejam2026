using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState CurrentState { get; private set; }

    public rounds CurrentRound { get; private set; } = rounds.round1;
    public int CurrentAttemptDialogue { get; private set; } = 5;
    public int Deaths {  get; private set; }
    public bool KillerIndentified { get; private set; }
    public bool IsEndGame { get; private set; }

    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] NarrativeDialogueData introDialogue;
    [SerializeField] NarrativeDialogueData badEnding;
    [SerializeField] NarrativeDialogueData goodEndingEarly;
    [SerializeField] NarrativeDialogueData goodEndingLate;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        RoundManager.Instance.OnAllRoundsCompleted += EvaluateGame;
        dialogueUI.OnDialogueEnded += OnIntroFinished;
        dialogueUI.StartNarrative(introDialogue);
    }

    public void NextRound()
    {
        if (CurrentRound < rounds.round3)
            CurrentRound++;

        if (CurrentAttemptDialogue > 0)
            CurrentAttemptDialogue--;
    }

    public void SetCurrentRound(rounds round)
    {
        CurrentRound = round;
    }

    public void SetState(GameState state)
    {
        CurrentState = state;
    }

    public void StartGamePlay()
    {
        SetState(GameState.Gameplay);
        RoundManager.Instance.StartFirstRound();
    }

    public void RegisterDeath()
    {
        Deaths++;
    }

    public void IdentifyKiller()
    {
        KillerIndentified = true;
    }

    void OnIntroFinished()
    {
        dialogueUI.OnDialogueEnded -= OnIntroFinished;
        dialogueUI.narrativeButton.gameObject.SetActive(false);
        GameManager.instance.StartGamePlay();
    }

    public void EvaluateGame()
    {
        if (!KillerIndentified)
            EndGame(badEnding);

        if (Deaths == 1)
            EndGame(goodEndingEarly);

        EndGame(goodEndingLate);
    }

    void EndGame(NarrativeDialogueData ending)
    {
        GameManager.instance.IsEndGame = true;
        GameManager.instance.SetState(GameState.Narrative);
        dialogueUI.StartNarrative(ending);
    }
}
