using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public CharacterDialogueData dialogueData;

    private DialogueMode currentMode;
    private int lineIndex;
    private bool pressureUnlocked;

    private DialogueRoundData roundData;
    private List<string> currentLines;

    // ===== INICIO =====
    public void StartDialogue()
    {
        roundData = dialogueData.rounds
            .Find(r => r.round == GameManager.instance.CurrentRound);

        if (roundData == null)
        {
            Debug.LogError($"[CharacterDialogue] No hay diálogo data para {dialogueData.npcName} en ronda {GameManager.instance.CurrentRound}");
            return;
        }

        currentMode = DialogueMode.Question;
        lineIndex = 0;
        pressureUnlocked = false;

        LoadCurrentSequence();
    }

    // ===== TEXTO ACTUAL =====
    public string GetCurrentLine()
    {
        if (currentLines == null || currentLines.Count == 0)
            return "...";

        return currentLines[Mathf.Clamp(lineIndex, 0, currentLines.Count - 1)];
    }

    // ===== BOTÓN PREGUNTAR =====
    public bool AdvanceQuestion()
    {
        lineIndex++;

        if (lineIndex < currentLines.Count)
            return true;

        // Fin de Question desbloquea Presionar
        pressureUnlocked = true;
        return false;
    }

    // ===== BOTÓN PRESIONAR =====
    public bool StartPressure()
    {
        if (!pressureUnlocked)
            return false;

        currentMode = DialogueMode.Pressure;
        lineIndex = 0;
        LoadCurrentSequence();
        pressureUnlocked = false;

        return currentLines != null && currentLines.Count > 0;
    }

    // ===== CARGA =====
    void LoadCurrentSequence()
    {
        if (roundData == null)
            return;

        var seq = roundData.dialogueSequences
            .Find(d => d.mode == currentMode);

        currentLines = seq != null ? seq.lines : null;
    }

    // ===== UTIL =====
    public bool CanPressure() => pressureUnlocked;
    public DialogueMode GetMode() => currentMode;
}