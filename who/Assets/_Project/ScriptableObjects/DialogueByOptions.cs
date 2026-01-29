using UnityEngine;

[CreateAssetMenu(fileName = "DialogueByOptions", menuName = "Scriptable Objects/DialogueByOptions")]
[System.Serializable]
public class DialogueByOptions : ScriptableObject
{
    public OptionsManager optionsManager;

    [TextArea(2, 5)]
    public string text;
}
