using System.Collections;
using StarterAssets;
using TMPro;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    [SerializeField] int timePenalty;
    [SerializeField] GameObject phoneRingingUI, invisibleWallsParent;

    [SerializeField] string[] firstCallLineSequence;

    public bool firstCallPlaying = true;

    DialogueManager dialogueManager;
    ScoreManager scoreManager;
    bool secondCallStarted = false;
    TextMeshProUGUI phoneText;
    Crosshair crosshair;

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        phoneText = phoneRingingUI.GetComponent<TextMeshProUGUI>();
        crosshair = FindAnyObjectByType<Crosshair>();
    }

    public void PickUpFirstCall()
    {
        GetComponent<Interaction>().canBeInteractedWith = false;
        crosshair.SetCrosshairMode("idle");

        GetComponent<Animator>().SetBool("isRinging", false);
        dialogueManager.PlayBlocks(firstCallLineSequence);
        GetComponent<AudioSource>().Stop();
        StartCoroutine(DelayGameStart());
    }

    IEnumerator DelayGameStart()
    {
        yield return new WaitForSecondsRealtime(dialogueManager.GetBlocksTotalLength(firstCallLineSequence));
        EndFirstCall();
    }

    private void EndFirstCall()
    {
        firstCallPlaying = false;
        phoneText.text = " ";
        invisibleWallsParent.gameObject.SetActive(false);
        scoreManager.StartTimer();
    }

    public void EndSecondCall()
    {
        GetComponent<Interaction>().canBeInteractedWith = false;
        crosshair.SetCrosshairMode("idle");
        if (!secondCallStarted) { return; }
        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        GetComponent<AudioSource>().Stop();
    }

    public void StartSecondCall()
    {
        GetComponent<Interaction>().canBeInteractedWith = true;
        secondCallStarted = true;
        GetComponent<Animator>().SetBool("isRingingSecondTime", true);
        GetComponent<AudioSource>().Play();
    }

    public void FailSecondCall()
    {
        GetComponent<Interaction>().canBeInteractedWith = false;
        if (FindAnyObjectByType<PlayerActions>().interaction == GetComponent<Interaction>()) 
        {
            crosshair.SetCrosshairMode("idle");
        }

        secondCallStarted = false;
        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        scoreManager.SkipTime(timePenalty);
        phoneText.text = "";
    }
}
