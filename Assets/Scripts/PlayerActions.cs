using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Transform playerCamera;
    public float interactionDistance;

    MenuManager menuManager;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    private void Update()
    {
        HandleInteraction();
    }

    public void HandleInteraction()
    {
        if (!menuManager.isPaused)
        {
            RaycastHit hit;
            Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactionDistance);

            if (hit.transform == null)
            {
                return;
            }
            //check if looking at interactable object
            if (hit.transform.GetComponent<Interaction>() != null)
            {
                //TODO: process observation
                Debug.Log("looking at" + hit.transform.gameObject.name);
                hit.transform.GetComponent<Interaction>().Observe();
                //process interaction
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.GetComponent<Interaction>().Interact();
                }
            }
        }
    }
}
