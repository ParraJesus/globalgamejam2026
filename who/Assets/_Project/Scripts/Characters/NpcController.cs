using UnityEngine;

public class NpcController : MonoBehaviour, IInteractive
{
    public DialogueUI dialogueUI;
    public string NPCID {  get; private set; }
    public bool IsTalking { get; private set; }

    void Start()
    {
        NPCID ??= GlobalHelper.GenerateUniqueID(gameObject);
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
        }
    }
}
