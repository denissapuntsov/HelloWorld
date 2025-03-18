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

    [SerializeField] public Interaction interaction;
    private void Start()
    {
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
        Debug.Log(other);
        interaction = other.gameObject.GetComponent<Interaction>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interaction>() != null)
        {
            Debug.Log(other);
            interaction = null;
        }
    }

    public void HandleInteraction()
    {
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
