using TMPro;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private CharacterDialogue currentCharacter;

    public void SetCharacter(CharacterDialogue character)
    {
        currentCharacter = character;
        dialogueText.text = "¿Qué deseas preguntar?";
    }

    public void OnOptionSelected(int optionIndex)
    {
        OptionsManager option = (OptionsManager)optionIndex;
        dialogueText.text = currentCharacter.GetDialogue(option);
    }
}
