using UnityEngine;

public class Hands : MonoBehaviour
{
    public GameObject heldObject;
    public Vector3 heldOffset;
    public Transform playerCamera;

    [field: SerializeField] 
    public bool HandsFull { get; set; } = false;

    PlayerActions actions;
    MenuManager menuManager;

    private void Start()
    {
        actions = FindAnyObjectByType<PlayerActions>();
        menuManager = FindAnyObjectByType<MenuManager>();
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        if (menuManager.isInMenu) { return; }

        if (HandsFull && Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            // 7 is the index of Block Dropping Items layer mask 
            Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 3, 7);
            if (hit.transform != null) { return; }

            RemoveHeldObject();
        }

        if (!HandsFull && Input.GetKeyUp(KeyCode.E))
        {
            actions.canInteract = true;
        }
    }

    public void AssignHeldObject(GameObject newObject)
    {
        if (!HandsFull)
        {

            heldObject = newObject;
            heldObject.layer = 3;
            heldObject.transform.localPosition = heldOffset;
            SetPhysics(false);

            heldObject.transform.SetParent(transform, false);
            HandsFull = true;

            actions.canInteract = false;
        }
        else
        {
            Debug.Log("BRUH");
        }
    }

    private void SetPhysics(bool state)
    {
        heldObject.GetComponent<Rigidbody>().useGravity = state;
        heldObject.GetComponent<Rigidbody>().isKinematic = !state;
        heldObject.GetComponent<BoxCollider>().enabled = state;
    }

    public void RemoveHeldObject()
    {
        if (heldObject != null)
        {

            heldObject.layer = 0;
            SetPhysics(true);

            heldObject.transform.SetParent(null, true);
            heldObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            heldObject = null;
            HandsFull = false;
        }
    }
}
