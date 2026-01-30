using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public TextMeshProUGUI npcName;

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

    private CharacterDialogue currentCharacter;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLine;

    // Narrative Dialogue
    private List<string> narrativeLines;
    private int narrativeIndex;

    public event Action OnDialogueEnded;

    private NpcController currentNPC;

    public void SetCharacter(CharacterDialogue character)
    {
        ActivateButtons();
        currentCharacter = character;
        currentCharacter.StartDialogue();

        portraitImage.sprite = character.dialogueData.npcPortrait;
        npcName.text = character.dialogueData.npcName;

        pressureButton.gameObject.SetActive(false);

        ShowCurrentLine();
        gameObject.SetActive(true);
    }

    void ActivateButtons()
    {
        closeButton.gameObject.SetActive(true);
        questionButton.gameObject.SetActive(true);
        portraitImage.gameObject.SetActive(true);
        npcName.gameObject.SetActive(true);
        narrativeButton.gameObject.SetActive(false);
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
        gameObject.SetActive(false);
        currentCharacter = null;
        OnDialogueEnded?.Invoke();
    }

    public void StartNarrative(NarrativeDialogueData data)
    {
        GameManager.instance.SetState(GameState.Narrative);
        narrativeLines = data.lines;
        narrativeIndex = 0;

        portraitImage.gameObject.SetActive(false);
        npcName.gameObject.SetActive(false);
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
