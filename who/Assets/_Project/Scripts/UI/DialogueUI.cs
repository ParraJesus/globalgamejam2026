using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;

    [Header("Buttons")]
    public Button questionButton;
    public Button pressureButton;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    private CharacterDialogue currentCharacter;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLine;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetCharacter(CharacterDialogue character)
    {
        currentCharacter = character;
        currentCharacter.StartDialogue();

        portraitImage.sprite = character.dialogueData.npcPortrait;
        pressureButton.gameObject.SetActive(false);

        ShowCurrentLine();
        gameObject.SetActive(true);
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
    }

}
