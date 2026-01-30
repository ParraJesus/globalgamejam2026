using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingsManager : MonoBehaviour
{

    private DialogueUI dialogueUI;

    [SerializeField] NarrativeDialogueData badEnding;
    [SerializeField] NarrativeDialogueData goodEndingEarly;
    [SerializeField] NarrativeDialogueData goodEndingLate;

    [SerializeField] Button exitButton;

    private void Awake()
    {
        dialogueUI = DialogueUI.Instance;

        if (dialogueUI != null)
        {
            dialogueUI.OnDialogueEnded += OnEndingFinished;
        }
        else
        {
            Debug.LogError("DialogueUI Instance no encontrado.");
        }
    }

    private void Start()
    {
        exitButton.gameObject.SetActive(false);

        switch (GameManager.instance.CurrentEnding)
        {
            case EndingState.Bad:
                dialogueUI.StartNarrative(badEnding);
                break;

            case EndingState.GoodEarly:
                dialogueUI.StartNarrative(goodEndingEarly);
                break;

            case EndingState.GoodLate:
                dialogueUI.StartNarrative(goodEndingLate);
                break;
        }
    }

    void OnEndingFinished()
    {
        dialogueUI.OnDialogueEnded -= OnEndingFinished;
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        yield return ScreenFader.instance.FadeOut();
        exitButton.gameObject.SetActive(true);
    }
}
