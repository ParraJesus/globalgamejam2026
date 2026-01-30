using UnityEngine;
using TMPro;
using System;

public class TimerManager: MonoBehaviour
{
    public static TimerManager instance;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    private float timeSet;
    public bool IsRunning { get; private set; }

    public event Action OnTimerFinished;

    private void Awake()
    {
        instance = this;
        timeSet = remainingTime;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (GameManager.instance.CurrentState != GameState.Gameplay)
            return;

        IsRunning = true;
        UpdateUI();
    }

    void Update()
    {
        if (!IsRunning) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            IsRunning = false;
            timerText.color = Color.red;

            OnTimerFinished?.Invoke();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ResetTimer()
    {
        remainingTime = timeSet;
        timerText.color = Color.white;
        IsRunning = true;
        UpdateUI();
    }

    public void BadAccuse()
    {
        remainingTime -=  remainingTime / 5;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void SetIsRunning(bool running)
    {
        IsRunning = running;
    }
}
