using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDialogueData", menuName = "Scriptable Objects/CharacterDialogueData")]
public class CharacterDialogueData : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public List<DialogueRoundData> rounds;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
}
