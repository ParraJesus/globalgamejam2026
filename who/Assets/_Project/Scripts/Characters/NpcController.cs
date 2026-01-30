using UnityEngine;

public class NpcController : MonoBehaviour, IInteractive
{
    private DialogueUI dialogueUI;
    public string NPCID {  get; private set; }
    public bool IsTalking { get; private set; }

    [SerializeField] public bool isKiller;

    [SerializeField] public NarrativeDialogueData AccuseDialog;

    void Awake()
    {
        NPCID ??= GlobalHelper.GenerateUniqueID(gameObject);

        dialogueUI = DialogueUI.Instance;

        if (dialogueUI != null) 
        {
            dialogueUI.OnDialogueEnded += EndTalking;
        }
        else
        {
            Debug.LogError("DialogueUI Instance no encontrado.");
        }
    }

    private void OnDestroy()
    {
        if (dialogueUI != null)
            dialogueUI.OnDialogueEnded -= EndTalking;
    }

    public bool CanInteract()
    {
        return !IsTalking;
    }

    public void Interact()
    {
        if (IsTalking) return;
        Talk();
    }

    private void Talk()
    {
        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI no asignado en " + gameObject.name);
            return;
        }

        SetIsTalking(true);
    }

    public void SetIsTalking(bool talking)
    {
        IsTalking = talking;

        if (IsTalking)
        {
            dialogueUI.gameObject.SetActive(true);
            dialogueUI.SetCharacter(GetComponent<CharacterDialogue>());
            dialogueUI.SetTarget(GetComponent<NpcController>());
        }
    }

    void EndTalking()
    {
        IsTalking = false;
    }

    public void ForceReset()
    {
        IsTalking = false;
    }
}
