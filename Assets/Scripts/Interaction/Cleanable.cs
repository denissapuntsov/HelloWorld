using UnityEngine;

public class Cleanable : MonoBehaviour
{
    [SerializeField] int pointsToAward = 0;
    ScoreManager scoreManager;
    bool isCleaning;

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public void StartCleaning()
    {
        GetComponent<Animator>().SetBool("IsCleaning", true);
    }

    public void StopCleaning()
    {
        GetComponent<Animator>().SetBool("IsCleaning", false);
    }

    public void FinishCleaning()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animator>().SetBool("IsCleaning", false);
        GetComponent<Interaction>().canBeInteractedWith = false;
        FindAnyObjectByType<Crosshair>().SetCrosshairMode("idle");
    }

    public void AwardPoints()
    {
        scoreManager.AddPoints(pointsToAward);
    }
}
