using UnityEngine;

public class Cleanable : MonoBehaviour
{
    [SerializeField] int pointsToAward = 0;
    [SerializeField] GameObject particlePrefab;
    [SerializeField] Transform particleParent;

    ScoreManager scoreManager;
    bool isCleaning;
    GameObject activeParticles;

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public void StartCleaning()
    {
        GetComponent<Animator>().SetBool("IsCleaning", true);
        activeParticles = Instantiate(particlePrefab, particleParent);
    }

    public void StopCleaning()
    {
        GetComponent<Animator>().SetBool("IsCleaning", false);
        activeParticles.GetComponent<ParticleSystem>().Stop();
    }

    public void FinishCleaning()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animator>().SetBool("IsCleaning", false);
        GetComponent<Interaction>().canBeInteractedWith = false;
        FindAnyObjectByType<Crosshair>().SetCrosshairMode("idle");
        activeParticles.GetComponent<ParticleSystem>().Stop();

    }

    public void AwardPoints()
    {
        scoreManager.AddPoints(pointsToAward);
    }
}
