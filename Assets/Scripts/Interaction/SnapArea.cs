using UnityEngine;

public class SnapArea : MonoBehaviour
{
    [SerializeField] GameObject objectToCheckFor, objectToShow;
    [SerializeField] int scoreCount = 0;

    Hands hands;
    ScoreManager scoreManager;

    private void Start()
    {
        hands = FindAnyObjectByType<Hands>();
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Holdable") && other.gameObject == objectToCheckFor)
        {
            SnapObject(other);
        }
    }

    private void SnapObject(Collider other)
    {
        hands.RemoveHeldObject();
        Destroy(other.gameObject);
        objectToShow.SetActive(true);
        scoreManager.AddPoints(scoreCount);
    }
}
