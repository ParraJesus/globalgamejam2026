using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeDialogueData", menuName = "Scriptable Objects/NarrativeDialogueData")]
public class NarrativeDialogueData : ScriptableObject
{
    [TextArea(4, 10)]
    public List<string> lines;
}
