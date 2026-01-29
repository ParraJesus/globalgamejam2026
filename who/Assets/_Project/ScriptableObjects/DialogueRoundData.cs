using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueRoundData", menuName = "Scriptable Objects/DialogueRoundData")]
public class DialogueRoundData : ScriptableObject
{
    public rounds round;
    public List<DialogueByOptions> dialogues;
}
