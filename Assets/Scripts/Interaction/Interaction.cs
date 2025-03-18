using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(VoiceTriggerData))]

public class Interaction : MonoBehaviour
{
    [SerializeField] bool includesRegularInteractionInFirst = true;
    
    public bool canBeInteractedWith = true;
    bool isFirstInteraction = true;

    public UnityEvent onObserve;
    public UnityEvent onTriggerFirstInteraction;
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
            if (isFirstInteraction)
            {
                // do a special single-time interaction
                onTriggerFirstInteraction?.Invoke();
                
                // do the regular interaction
                if (includesRegularInteractionInFirst) { onInteract?.Invoke(); }
                isFirstInteraction = false;
            }
            else
            {
                // process interactions as normal
                onInteract?.Invoke();
            }
        }
    }

    public void StopInteraction()
    {
        onStopInteraction?.Invoke();
    }
}
