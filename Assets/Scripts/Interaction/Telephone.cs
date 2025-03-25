using System.Collections;
using StarterAssets;
using TMPro;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    [SerializeField] int timePenalty;
    [SerializeField] GameObject invisibleWallsParent, tutorialUI;

    [SerializeField] string[] firstCallLineSequence, secondCallSuccess, secondCallFail;

    public bool firstCallPlaying = true;

    DialogueManager dialogueManager;
    ScoreManager scoreManager;
    MenuManager menuManager;
    ObjectiveManager objectiveManager;
    public bool firstCallEnded, secondCallStarted = false;
    //TextMeshProUGUI phoneText;
    Crosshair crosshair;

    string telephoneObjective = "Pick up the telephone";

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        menuManager = FindAnyObjectByType<MenuManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        objectiveManager = FindAnyObjectByType<ObjectiveManager>();

        crosshair = FindAnyObjectByType<Crosshair>();

        objectiveManager.AddObjective(telephoneObjective);
    }

    public void PickUpFirstCall()
    {
        menuManager.isPlayerFrozenExternally = true;
        menuManager.SetPlayerMovement(false);
        GetComponent<Interaction>().canBeInteractedWith = false;
        crosshair.SetCrosshairMode("idle");

        GetComponent<Animator>().SetBool("isRinging", false);
        dialogueManager.PlayBlocks(firstCallLineSequence);
        GetComponent<AudioSource>().mute = true;
        StartCoroutine(DelayGameStart());

        objectiveManager.RemoveObjective(telephoneObjective);
    }

    IEnumerator DelayGameStart()
    {
        yield return new WaitForSecondsRealtime(dialogueManager.GetBlocksTotalLength(firstCallLineSequence));
        EndFirstCall();
    }

    private void EndFirstCall()
    {
        if (firstCallEnded) { return; }
        firstCallEnded = true;
        Debug.Log("Ended first call");
        tutorialUI.SetActive(true);
        menuManager.isPlayerFrozenExternally = false;
        menuManager.SetPlayerMovement(true);
        firstCallPlaying = false;
        invisibleWallsParent.gameObject.SetActive(false);
        scoreManager.StartTimer();
    }

    public void SkipFirstCall()
    {
        StopCoroutine(DelayGameStart());
        EndFirstCall();
    }

    public void EndSecondCall()
    {
        objectiveManager.RemoveObjective(telephoneObjective);
        GetComponent<Interaction>().canBeInteractedWith = false;
        crosshair.SetCrosshairMode("idle");
        if (!secondCallStarted) { return; }
        dialogueManager.PlayBlocks(secondCallSuccess);
        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        GetComponent<AudioSource>().Stop();
    }

    public void StartSecondCall()
    {
        GetComponent<Interaction>().canBeInteractedWith = true;
        secondCallStarted = true;
        GetComponent<Animator>().SetBool("isRingingSecondTime", true);
        GetComponent<AudioSource>().mute = false;
        GetComponent<AudioSource>().Play(0);
        objectiveManager.AddObjective(telephoneObjective);
    }

    public void FailSecondCall()
    {
        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        GetComponent<AudioSource>().Stop();
        objectiveManager.RemoveObjective(telephoneObjective);
        GetComponent<Interaction>().canBeInteractedWith = false;
        if (FindAnyObjectByType<PlayerActions>().interaction == GetComponent<Interaction>()) 
        {
            crosshair.SetCrosshairMode("idle");
        }
        dialogueManager.PlayBlocks(secondCallFail);
        secondCallStarted = false;

        GetComponent<Animator>().SetBool("isRingingSecondTime", false);
        scoreManager.SkipTime(timePenalty);
    }
}
