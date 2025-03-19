using System;
using System.Collections;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public GameObject heldObject;
    public Vector3 heldOffset;
    public Transform playerCamera;

    CollisionCheck checker;

    [field: SerializeField] 
    public bool HandsFull { get; set; } = false;

    public bool dropEnabled, isFacingObstacles = false;

    PlayerActions actions;
    MenuManager menuManager;

    private void Start()
    {
        actions = FindAnyObjectByType<PlayerActions>();
        menuManager = FindAnyObjectByType<MenuManager>();
        playerCamera = Camera.main.transform;
        checker = FindAnyObjectByType<CollisionCheck>();
    }

    private void Update()
    {
        if (menuManager.isInMenu) { return; }

        if (dropEnabled && !isFacingObstacles && HandsFull && Input.GetKeyDown(KeyCode.E))
        {
            //RaycastHit hit;
            //Debug.Log("started raycast for drop");

            //Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 3, 2);
            //if (hit.transform != null) { Debug.Log($"{hit.transform.gameObject.name} is in the way of ray"); return; }
            //Debug.Log("empty space is located by ray");

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
            checker.gameObject.SetActive(true);
            heldObject = newObject;
            heldObject.layer = 2;
            heldObject.transform.localPosition = heldObject.GetComponent<Holdable>().heldOffset;
            SetPhysics(false);

            heldObject.transform.SetParent(transform, false);
            HandsFull = true;

            actions.canInteract = false;
            StartCoroutine(DelayItemDrop());
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
            heldObject.transform.position = checker.gameObject.transform.position;
            heldObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            heldObject.transform.SetParent(null, true);

            checker.gameObject.SetActive(false);
            heldObject = null;
            HandsFull = false;
            dropEnabled = false;
        }
    }

    IEnumerator DelayItemDrop()
    {
        yield return new WaitForEndOfFrame();
        dropEnabled = true;
    }
}
