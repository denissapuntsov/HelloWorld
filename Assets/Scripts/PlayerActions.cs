using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public bool canInteract = true;
    public Transform playerCamera;
    public float interactionDistance;


    private void Update()
    {
        HandleInteraction();
    }

    public void HandleInteraction()
    {
        if (canInteract)
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
                //process observation
                hit.transform.GetComponent<Interaction>().Observe();
                //process interaction
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.GetComponent<Interaction>().Interact();
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    hit.transform.GetComponent<Interaction>().StopInteraction();
                }
            }
        }
    }
}
