using TMPro;
using UnityEngine;

public class AttemptsManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI attemptsText;

    private void Start()
    {
        GameManager.instance.OnAttemptsChanged += UpdateUI;
        UpdateUI(GameManager.instance.CurrentAttemptDialogue);
    }

    void UpdateUI(int attempts)
    {
        attemptsText.text = $"Intentos: {attempts}";
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnAttemptsChanged -= UpdateUI;
    }
}
