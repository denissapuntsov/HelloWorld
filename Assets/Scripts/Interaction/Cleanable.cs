using UnityEngine;

public class Cleanable : MonoBehaviour
{
    [SerializeField] int pointsToAward = 0;
    [SerializeField] GameObject cleanupParticlePrefab, successParticlePrefab;
    [SerializeField] AudioClip cleaningClip, finishedCleaningClip;
    [SerializeField] Transform particleParent;
    [SerializeField] AudioSource globalSFXSource;

    ScoreManager scoreManager;
    bool isCleaning;
    GameObject cleanupParticles;

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    public void StartCleaning()
    {
        GetComponent<Animator>().SetBool("IsCleaning", true);
        globalSFXSource.clip = cleaningClip;
        globalSFXSource.Play();
        cleanupParticles = Instantiate(cleanupParticlePrefab, particleParent);
    }

    public void StopCleaning()
    {
        GetComponent<Animator>().SetBool("IsCleaning", false);
        globalSFXSource.mute = true;
        globalSFXSource.Stop();
        globalSFXSource.mute = false;
        if (cleanupParticles != null) { cleanupParticles.GetComponent<ParticleSystem>().Stop(); }
    }

    public void FinishCleaning()
    {
        globalSFXSource.PlayOneShot(finishedCleaningClip);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animator>().SetBool("IsCleaning", false);
        GetComponent<Interaction>().canBeInteractedWith = false;
        FindAnyObjectByType<Crosshair>().SetCrosshairMode("idle");
        cleanupParticles.GetComponent<ParticleSystem>().Stop();
        Instantiate(successParticlePrefab, particleParent);
    }

    public void AwardPoints()
    {
        scoreManager.AddPoints(pointsToAward);
    }
}
