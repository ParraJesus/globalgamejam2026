using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Scriptable Objects/DialogueSequence")]
[System.Serializable]
public class DialogueSequence : ScriptableObject
{
    public DialogueMode mode;

    [TextArea(2, 5)]
    public List<string> lines;
}
