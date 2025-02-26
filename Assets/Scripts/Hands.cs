using UnityEngine;

public class Hands : MonoBehaviour
{
    public GameObject heldObject;
    public Vector3 heldOffset;
    [SerializeField] private bool handsFull = false;

    public void AssignHeldObject(GameObject newObject)
    {
        if (!handsFull)
        {
            heldObject = newObject;
            heldObject.transform.localPosition = heldOffset;
            heldObject.GetComponent<Rigidbody>().useGravity = false;
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.transform.SetParent(transform, false);
            handsFull = true;
        }
        else
        {
            Debug.Log("BRUH");
        }
    }

    public void RemoveHeldObject()
    {
        heldObject.GetComponent<Rigidbody>().useGravity = true;
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject = null;
    }
}
