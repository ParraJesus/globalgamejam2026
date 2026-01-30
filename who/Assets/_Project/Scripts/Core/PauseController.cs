using UnityEngine;

public class PauseController: MonoBehaviour
{
    public static bool isDetectiveInDialogue { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        isDetectiveInDialogue = pause;
    }
}
