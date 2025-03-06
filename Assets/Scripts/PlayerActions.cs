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


        if (!canInteract)
        {
            return;
        }

        RaycastHit hit;
        Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactionDistance);

        if (hit.transform == null)
        {
            return;
        }

        //check if looking at interactable object
        Interaction interaction = hit.transform.GetComponent<Interaction>();

        if (interaction != null)
        {
            //process observation
            interaction.Observe();
            //process interaction
            if (Input.GetKeyDown(KeyCode.E))
            {
                interaction.Interact();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                interaction.StopInteraction();
            }
        }
    }
}
