using System;
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
    [SerializeField] Sprite sprite;

    public event Action<int> OnAttemptsChanged;

    public EndingState CurrentEnding { get; private set; }

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
        SetState(GameState.Narrative);

        RoundManager.Instance.OnAllRoundsCompleted += EvaluateGame;

        dialogueUI.OnDialogueEnded += OnIntroFinished;
        dialogueUI.npcName.text = "Parra";
        dialogueUI.StartNarrative(introDialogue);
    }

    public void SetCurrentRound(rounds round)
    {
        CurrentRound = round;
        ResetAttemptsForRound();
    }

    void ResetAttemptsForRound()
    {
        switch (CurrentRound)
        {
            case rounds.round1:
                CurrentAttemptDialogue = 5;
                break;
            case rounds.round2:
                CurrentAttemptDialogue = 4;
                break;
            case rounds.round3:
                CurrentAttemptDialogue = 3;
                break;
        }

        OnAttemptsChanged?.Invoke(CurrentAttemptDialogue);
    }

    public bool ConsumeDialogueAttempt()
    {
        if (CurrentAttemptDialogue <= 0)
            return false;

        CurrentAttemptDialogue--;
        OnAttemptsChanged?.Invoke(CurrentAttemptDialogue);
        return true;
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
        {
            SetEnding(EndingState.Bad);
            return;
        }

        if (Deaths == 1)
        {
            SetEnding(EndingState.GoodEarly);
            return;
        }

        SetEnding(EndingState.GoodLate);
    }

   void SetEnding(EndingState ending)
    {
        CurrentEnding = ending;
        IsEndGame = true;
        SetState(GameState.Narrative);

        StartCoroutine(LoadEndingScene());
    }

    IEnumerator LoadEndingScene()
    {
        yield return ScreenFader.instance.FadeOut();

        UnityEngine.SceneManagement.SceneManager.LoadScene("Endings");
        SetPortrait(sprite);

        yield return ScreenFader.instance.FadeIn();
    }

    public void SetPortrait(Sprite sprite)
    {
        dialogueUI.portraitImage.sprite = sprite;
        dialogueUI.npcName.text = "Parra";
    }
}
