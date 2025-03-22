using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
// collider center (0, 0, 0.9), size (0.4, 0.56, 1.35)

public class PlayerActions : MonoBehaviour
{
    public bool canInteract = true;
    public Transform playerCamera;
    public float interactionDistance;
    Telephone telephone;
    Hands hands;
    Crosshair crosshair;

    [SerializeField] public Interaction interaction;
    private void Start()
    {
        crosshair = FindAnyObjectByType<Crosshair>();
        hands = FindAnyObjectByType<Hands>();
        telephone = FindAnyObjectByType<Telephone>();
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if telephone is ringing for the first time, limit interaction to telephone only
        if (telephone.firstCallPlaying && other.gameObject.GetComponent<Telephone>() == null) { return; }

        if (other.gameObject.GetComponent<Interaction>() == null || hands.HandsFull) { return; }
        interaction = other.gameObject.GetComponent<Interaction>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interaction>() != null)
        {
            Debug.Log(other);
            interaction = null;
            crosshair.SetCrosshairMode("idle");
        }
    }

    public void HandleInteraction()
    {
        if (interaction == null)
        {
            crosshair.SetCrosshairMode("idle");
            return;
        }

        //process observation
        Debug.Log($"Observing {interaction}");
        if (interaction.canBeInteractedWith)
        {
            interaction.Observe();
        }
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
