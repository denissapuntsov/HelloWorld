using UnityEngine;

[RequireComponent(typeof(Interaction))]
public class Holdable : MonoBehaviour
{
    public Vector3 heldOffset = new Vector3(0.5f, 1, 2);
    public Quaternion heldOffsetRotation;
}
