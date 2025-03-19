using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

public class CollisionCheck : MonoBehaviour
{
    Hands hands;

    private void Start()
    {
        hands = FindAnyObjectByType<Hands>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("IgnoreCollisionChecker") || other.gameObject.CompareTag("Feet")) { return; }

        hands.isFacingObstacles = true;
    }

    private void OnTriggerExit(Collider other)
    {
        hands.isFacingObstacles = false;
    }
}
