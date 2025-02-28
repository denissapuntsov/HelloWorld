using UnityEngine;

public class Hands : MonoBehaviour
{
    public GameObject heldObject;
    public Vector3 heldOffset;
    [SerializeField] public bool handsFull = false;

    PlayerActions actions;
    MenuManager menuManager;

    private void Start()
    {
        actions = FindAnyObjectByType<PlayerActions>();
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    private void Update()
    {
        if (!menuManager.isPaused && !menuManager.isInInventory)
        {
            if (handsFull && Input.GetKeyDown(KeyCode.E))
            {
                RemoveHeldObject();
            }

            if (!handsFull && Input.GetKeyUp(KeyCode.E))
            {
                actions.canInteract = true;
            }
        }
    }

    public void AssignHeldObject(GameObject newObject)
    {
        if (!handsFull)
        {
            heldObject = newObject;
            heldObject.layer = 3;
            heldObject.transform.localPosition = heldOffset;

            heldObject.GetComponent<Rigidbody>().useGravity = false;
            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.GetComponent<BoxCollider>().enabled = false;

            heldObject.transform.SetParent(transform, false);
            handsFull = true;

            actions.canInteract = false;
        }
        else
        {
            Debug.Log("BRUH");
        }
    }

    public void RemoveHeldObject()
    {
        heldObject.layer = 0;
        heldObject.GetComponent<Rigidbody>().useGravity = true;
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.GetComponent<BoxCollider>().enabled = true;

        heldObject.transform.SetParent(null, true);
        heldObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        heldObject = null;
        handsFull = false;
    }
}
