using System.Collections;
using TMPro;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    [SerializeField] int timePenalty;
    [SerializeField] GameObject phoneRingingUI;

    ScoreManager scoreManager;
    bool secondCallStarted = false;
    TextMeshProUGUI phoneText;

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        phoneText = phoneRingingUI.GetComponent<TextMeshProUGUI>();
    }

    public void EndFirstCall()
    {
        GetComponent<Animator>().SetBool("isRinging", false);
        phoneText.text = " ";
    }

    public void EndSecondCall()
    {
        if (secondCallStarted)
        {
            GetComponent<Animator>().SetBool("isRingingSecondTime", false);
            phoneText.text = " ";
        }
    }

    public void StartSecondCall()
    {
        secondCallStarted = true;
        GetComponent<Animator>().SetBool("isRingingSecondTime", true);
        phoneText.text = "Someone's calling the phone again!";
    }

    public void FailFirstCall()
    {
        EndFirstCall();
        scoreManager.SkipTime((int)(scoreManager.timeLimit / 2));
        scoreManager.StartTimer();
        phoneText.text = "";
    }

    public void FailSecondCall()
    {
        secondCallStarted = false;
        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        scoreManager.SkipTime(timePenalty);
        phoneText.text = "";
    }
}
