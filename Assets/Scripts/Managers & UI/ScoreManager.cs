using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private int score = 0;
    [SerializeField] private int threshold;
    [SerializeField] bool timeStarted, timeRanOut, switchedMusic, finishedCleaning = false;
    [SerializeField] TextMeshProUGUI timerUI, scoreUI;
    [SerializeField] Slider timerSlider, trashcanSlider;
    [SerializeField] GameObject endStateUI, endStateImage, fadeoutPanel, captions;
    [SerializeField] AudioSource globalDialogueAudioSource;

    [SerializeField] Animator endStateSpriteAnimator;
    public float timer;
    public float timeLimit = 480;
    public bool timerPaused = false;

    Telephone telephone;
    MenuManager menuManager;
    float minutes, seconds;
    string timeToString = "";
    TextMeshProUGUI endStateText;

    private void Start()
    {
        telephone = FindAnyObjectByType<Telephone>();
        menuManager = FindAnyObjectByType<MenuManager>();
        scoreUI.text = "Score: 0";
        endStateUI.SetActive(false);
        endStateText = endStateUI.GetComponentInChildren<TextMeshProUGUI>();
        timerSlider.maxValue = timeLimit;
    }

    private void Update()
    {
        if (menuManager.isPaused || timeRanOut || finishedCleaning) { return; }

        if (timeStarted && !timerPaused)
        {
            CountTime();
        }

        if (timer > timeLimit - 60 && !switchedMusic) 
        {
            switchedMusic = true;
            FindAnyObjectByType<MusicManager>().ShiftIntoHighGear();
        }
    }

    private void CountTime()
    {
        timer += Time.deltaTime;
        minutes = Mathf.Floor(timer / 60);
        seconds = Mathf.Floor(timer - minutes * 60);

        timeToString = minutes.ToString("00") + ":" + seconds.ToString("00");
        timerUI.text = timeToString;

        timerSlider.value = timer;

        if (timer >= timeLimit)
        {
            timerSlider.value = timeLimit;
            timeRanOut = true;
            EndGame();
        }
    }

    public void SkipTime(int timeInSeconds)
    {
        if (timer + timeInSeconds > timeLimit)
        {
            Debug.LogError($"Cannot add {timeInSeconds} seconds to the timer: result exceeds 10 minutes");
            return;
        }

        timer += timeInSeconds;
    }

    public void AddPoints(int points)
    {
        score += points;
        trashcanSlider.value = score;
        scoreUI.text = $"Score: {score}";

        if (score >= 50 && score < 60 && !telephone.secondCallStarted)
        {
            telephone.StartSecondCall();
        }

        if (score >= 100)
        {
            finishedCleaning = true;
            menuManager.SetGamePause(true);
            endStateUI.SetActive(true);
            EngageWinState(true);
        }
    }

    public void SubtractPoints(int points)
    {
        score -= points;
    }

    private void EndGame()
    {
        var menuManager = FindAnyObjectByType<MenuManager>();
        menuManager.HideUI(true);
        menuManager.gameHasFinished = true;
        menuManager.pauseUI.SetActive(false);
        menuManager.inventoryUI.SetActive(false);
        globalDialogueAudioSource.Stop();
        globalDialogueAudioSource.clip = null;
        captions.GetComponent<TextMeshProUGUI>().text = "";

        endStateUI.SetActive(true);
        FindAnyObjectByType<MusicManager>().GraduallyEndMusic();

        // Freeze player movements and camera position
        menuManager.SetPlayerMovement(false);
        FindAnyObjectByType<PlayerActions>().canInteract = false;
    }

    public void FinishEndStateTransition()
    {

        endStateImage.SetActive(true);
        // if time ran out and we haven't crossed the point threshold, lose (Ending 1)
        if (timeRanOut && score < threshold)
        {
            EngageLoseState();
        }

        // if time ran out and the score is above point threshold, but not at 100%, win (Ending 2)
        if (timeRanOut && threshold < score && score < 100)
        {
            EngageWinState(false);
        }
    }

    public void EngageWinState(bool isPerfectScore)
    {
        if (isPerfectScore)
        {
            StartCoroutine(StartDelayedFadeout(12.1f));
            endStateText.text = "Achieved Ending 3: Perfect Score.";
            Debug.Log("Achieved Ending 3: Perfect Score.");
            endStateSpriteAnimator.Play("Best_Ending");
            FindAnyObjectByType<DialogueManager>().PlayBlock("bestEnding");
            return;
        }
        StartCoroutine(StartDelayedFadeout(7.6f));
        endStateText.text = $"Achieved Ending 2 with score {score}.";
        Debug.Log($"Achieved Ending 2 with score {score}.");
        endStateSpriteAnimator.Play("Good_Ending");
        FindAnyObjectByType<DialogueManager>().PlayBlock("goodEnding");
    }

    public void EngageLoseState()
    {
        StartCoroutine(StartDelayedFadeout(7.2f));
        endStateText.text = $"Achieved Ending 1: score {score} below threshold {threshold}.";
        Debug.Log($"Achieved Ending 1: score {score} below threshold {threshold}.");
        endStateSpriteAnimator.Play("Fail_Ending");
        FindAnyObjectByType<DialogueManager>().PlayBlock("badEnding");
    }

    public void StartTimer()
    {
        timeStarted = true;
    }

    IEnumerator StartDelayedFadeout(float timeToDelay)
    {
        yield return new WaitForSeconds(timeToDelay);
        fadeoutPanel.GetComponent<Animator>().Play("fadeOut");
    }
}
