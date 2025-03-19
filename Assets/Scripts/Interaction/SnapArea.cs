using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]

public class SnapArea : MonoBehaviour
{
    [SerializeField] GameObject objectToCheckFor, objectToShow;
    [SerializeField] int scoreCount = 0;

    public UnityEvent onSnap;

    Hands hands;
    ScoreManager scoreManager;

    private void Start()
    {
        hands = FindAnyObjectByType<Hands>();
        scoreManager = FindAnyObjectByType<ScoreManager>();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CollisionCheck>() && hands.heldObject == objectToCheckFor)
        {
            SnapObject(hands.heldObject);
        }
    }

    private void SnapObject(GameObject obj)
    {
        hands.RemoveHeldObject();
        Destroy(obj);
        objectToShow.SetActive(true);
        scoreManager.AddPoints(scoreCount);

        onSnap?.Invoke();
    }
}
