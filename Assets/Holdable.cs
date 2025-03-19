using UnityEngine;

[RequireComponent(typeof(Interaction))]
[RequireComponent(typeof(SphereCollider))]
public class Holdable : MonoBehaviour
{
    public Vector3 heldOffset = new Vector3(0.5f, 1, 2);

    private void Start()
    {
        SphereCollider trigger = GetComponent<SphereCollider>();
        trigger.isTrigger = true;
    }   
}
