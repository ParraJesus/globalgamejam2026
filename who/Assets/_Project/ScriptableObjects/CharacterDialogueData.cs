using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDialogueData", menuName = "Scriptable Objects/CharacterDialogueData")]
public class CharacterDialogueData : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public AudioClip voiceSound;
    public List<DialogueRoundData> rounds;
}
