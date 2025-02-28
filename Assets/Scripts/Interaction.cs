using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public bool canBeInteractedWith = true;

    public UnityEvent onObserve;
    public UnityEvent onInteract;
    public UnityEvent onStopInteraction;

    public void Observe()
    {
        if (canBeInteractedWith)
        {
            onObserve?.Invoke();
        }
    }

    public void Interact()
    {
        if (canBeInteractedWith)
        {
            onInteract?.Invoke();
        }
    }

    public void StopInteraction()
    {
        onStopInteraction?.Invoke();
    }
}
