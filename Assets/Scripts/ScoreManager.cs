using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private int score = 0;
    [SerializeField] private int[] thresholds;
    [SerializeField] bool timeRanOut = false;
    [SerializeField] TextMeshProUGUI timerUI;
    public float timer;

    MenuManager menuManager;
    float minutes, seconds;
    string timeToString = "";
    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    private void Update()
    {
        if (menuManager.isPaused || timeRanOut) { return; }
        CountTime();
    }

    private void CountTime()
    {
        timer += Time.deltaTime;
        minutes = Mathf.Floor(timer / 60);
        seconds = Mathf.Floor(timer - minutes * 60);

        timeToString = minutes.ToString("00") + ":" + seconds.ToString("00");
        timerUI.text = timeToString;

        if (timer >= 600)
        {
            timeRanOut = true;
            EngageLoseState();
        }
    }

    public void AddPoints(int points)
    {
        score += points;
    }

    public void SubtractPoints(int points)
    {
        score -= points;
    }

    public void EngageWinState()
    {

    }

    public void EngageLoseState()
    {

    }
}
