using UnityEngine;

public class SnapArea : MonoBehaviour
{
    [SerializeField] GameObject objectToSnap;

    Hands hands;

    private void Start()
    {
        hands = FindAnyObjectByType<Hands>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Holdable") && other.gameObject == objectToSnap)
        {
            hands.RemoveHeldObject();
            other.gameObject.GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.transform.position = transform.position;

            other.transform.rotation = transform.rotation;
        }
    }
}
