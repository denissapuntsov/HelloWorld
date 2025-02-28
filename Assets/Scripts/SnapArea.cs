using UnityEngine;

public class SnapArea : MonoBehaviour
{
    [SerializeField] GameObject objectToSnap;
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
        if (other.CompareTag("Holdable") && other.gameObject == objectToSnap)
        {
            SnapObject(other);
        }
    }

    private void SnapObject(Collider other)
    {
        hands.RemoveHeldObject();
        other.gameObject.GetComponent<Interaction>().canBeInteractedWith = false;
        other.gameObject.GetComponent<Collider>().enabled = false;
        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        other.gameObject.GetComponent<Rigidbody>().useGravity = false;
        other.transform.position = transform.position;

        other.transform.rotation = transform.rotation;

        scoreManager.AddPoints(scoreCount);
    }
}
