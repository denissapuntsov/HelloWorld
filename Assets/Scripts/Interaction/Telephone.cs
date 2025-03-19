using System.Collections;
using TMPro;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    [SerializeField] int timePenalty;
    [SerializeField] GameObject phoneRingingUI, invisibleWallsParent;

    public bool firstCallPlaying = true;

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
        firstCallPlaying = false;
        GetComponent<Animator>().SetBool("isRinging", false);
        phoneText.text = " ";
        invisibleWallsParent.gameObject.SetActive(false);
        scoreManager.StartTimer();
        GetComponent<AudioSource>().Stop();
    }

    public void EndSecondCall()
    {
        if (!secondCallStarted) { return; }
        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        phoneText.text = " ";
        GetComponent<AudioSource>().Stop();
    }

    public void StartSecondCall()
    {
        secondCallStarted = true;
        GetComponent<Animator>().SetBool("isRingingSecondTime", true);
        phoneText.text = "Someone's calling the phone again!";
        GetComponent<AudioSource>().Play();
    }

    public void FailSecondCall()
    {
        secondCallStarted = false;
        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        scoreManager.SkipTime(timePenalty);
        phoneText.text = "";
    }
}
