using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public CharacterDialogueData dialogueData;

    public string GetDialogue(OptionsManager option)
    {
        var currentRound = GameManager.instance.CurrentRound;

        var roundData = dialogueData.rounds
            .Find(r => r.round == currentRound);

        if (roundData == null)
            return "El personaje guarda silencio...";

        var dialogue = roundData.dialogues
            .Find(d => d.optionsManager == option);

        return dialogue != null ? dialogue.text : "No responde a eso.";
    }
}
