using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public TextMeshProUGUI npcName;
    [SerializeField] public Sprite PlaceHolder;

    [Header("Buttons")]
    public Button questionButton;
    public Button pressureButton;
    public Button accuseButton;
    public Button narrativeButton;
    public Button closeButton;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    [Header("References")]
    [SerializeField] TimerManager timerManager;
    [SerializeField] CanvasGroup canvasGroup;

    private CharacterDialogue currentCharacter;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLine;

    // Narrative Dialogue
    private List<string> narrativeLines;
    private int narrativeIndex;

    public event Action OnDialogueEnded;

    private NpcController currentNPC;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    public void SetCharacter(CharacterDialogue character)
    {
        GameManager.instance.SetState(GameState.Gameplay);

        canvasGroup.alpha = 1;
        SetUIInteractable(true);

        currentCharacter = character;
        currentCharacter.StartDialogue();

        portraitImage.sprite = character.dialogueData.npcPortrait;
        npcName.text = character.dialogueData.npcName;

        pressureButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        accuseButton.gameObject.SetActive(false);
        questionButton.gameObject.SetActive(true);
        narrativeButton.gameObject.SetActive(false);

        portraitImage.gameObject.SetActive(true);
        npcName.gameObject.SetActive(true);

        gameObject.SetActive(true);
        ShowCurrentLine();
    }

    public void SetTarget(NpcController npc)
    {
        currentNPC = npc;
    }

    // ===== MOSTRAR TEXTO =====
    void ShowCurrentLine()
    {
        currentLine = currentCharacter.GetCurrentLine();
        StartTyping(currentLine);
    }

    // ===== BOTÓN PREGUNTAR =====
    public void OnQuestion()
    {
        if (isTyping)
        {
            CompleteLineInstantly();
            return;
        }

        bool hasMore = currentCharacter.AdvanceQuestion();

        if (!hasMore)
        {
            pressureButton.gameObject.SetActive(currentCharacter.CanPressure());
            accuseButton.gameObject.SetActive(currentCharacter.CanPressure());
            return;
        }

        ShowCurrentLine();
    }

    // ===== BOTÓN PRESIONAR =====
    public void OnPressure()
    {
        if (isTyping)
        {
            CompleteLineInstantly();
            return;
        }

        bool started = currentCharacter.StartPressure();

        if (!started)
        {
            EndDialogue();
            return;
        }

        pressureButton.gameObject.SetActive(false);
        ShowCurrentLine();
    }

    public void OnAccuse()
    {
        if (currentNPC == null) return;

        Debug.Log($"Current NPC: {currentNPC}");

        if (!GameManager.instance.ConsumeDialogueAttempt())
        {
            Debug.Log("No hay más intentos para hablar.");
            return;
        }

        if (currentNPC.isKiller)
        {
            GameManager.instance.IdentifyKiller();
            GameManager.instance.EvaluateGame();
            timerManager.StopTimer();
        }else
        {
            currentNPC.gameObject.SetActive(false);
            ScreenFader.instance.FadeOut();
            StartNarrative(currentNPC.AccuseDialog);
            ScreenFader.instance.FadeIn();
            timerManager.BadAccuse();
            GameManager.instance.SetState(GameState.Gameplay);
        }
    }

    public void OnClose()
    {
        currentNPC.SetIsTalking(false);

        portraitImage.sprite = PlaceHolder;
        npcName.text = "Parra";
    }

    // ===== TYPING =====
    void StartTyping(string line)
    {
        StopTyping();
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void CompleteLineInstantly()
    {
        StopTyping();
        dialogueText.text = currentLine;
        isTyping = false;
    }

    void StopTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    }

    // ===== FIN =====
    void EndDialogue()
    {
        StopTyping();
        SetUIInteractable(false);
        gameObject.SetActive(false);
        currentCharacter = null;
        currentNPC = null;
        OnDialogueEnded?.Invoke();
    }

    void SetUIInteractable(bool value)
    {
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }

    public void StartNarrative(NarrativeDialogueData data)
    {
        GameManager.instance.SetState(GameState.Narrative);

        SetUIInteractable(true);
        canvasGroup.alpha = 1;

        narrativeLines = data.lines;
        narrativeIndex = 0;

        questionButton.gameObject.SetActive(false);
        pressureButton.gameObject.SetActive(false);
        accuseButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        narrativeButton.gameObject.SetActive(true);

        gameObject.SetActive(true);
        ShowNarrativeLine();
    }

    void ShowNarrativeLine()
    {
        currentLine = narrativeLines[narrativeIndex];
        StartTyping(currentLine);
    }

    public void OnNarrativeNext()
    {
        if (isTyping)
        {
            CompleteLineInstantly();
            return;
        }

        narrativeIndex++;

        if (narrativeIndex >= narrativeLines.Count)
        {
            EndDialogue();
            return;
        }

        ShowNarrativeLine();
    }
}
