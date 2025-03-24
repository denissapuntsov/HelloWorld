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
    bool hasSnapped = false;

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
        if (hasSnapped) { return; }

        obj.GetComponent<Holdable>().isSnapped = true;
        hasSnapped = true;
        hands.RemoveHeldObject();
        Destroy(obj);
        if (objectToShow != null) { objectToShow.SetActive(true); }
        scoreManager.AddPoints(scoreCount);

        onSnap?.Invoke();
    }
}
