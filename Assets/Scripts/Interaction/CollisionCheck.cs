using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

[RequireComponent(typeof(BoxCollider))]

public class CollisionCheck : MonoBehaviour
{
    public GameObject obstacle;
    Hands hands;

    private void Start()
    {
        hands = FindAnyObjectByType<Hands>();
    }

    private void Update()
    {
        if (obstacle == null) { obstacle = null; hands.isFacingObstacles = false; return; }
        hands.isFacingObstacles = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("IgnoreCollisionChecker") || other.gameObject.CompareTag("Feet")) { return; }
        obstacle = other.gameObject;
        //hands.isFacingObstacles = true;
    }

    private void OnTriggerExit(Collider other)
    {
        obstacle = null;
    }

    public void ClearObstacle()
    {
        obstacle = null;
    }
}
