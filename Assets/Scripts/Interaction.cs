using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public UnityEvent onObserve;
    public UnityEvent onInteract;

    public void Observe()
    {
        onObserve?.Invoke();
    }

    public void Interact()
    {
        onInteract?.Invoke();
    }
}
