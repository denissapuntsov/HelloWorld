using UnityEngine;

[RequireComponent(typeof(Interaction))]
[RequireComponent(typeof(AudioSource))]
public class Holdable : MonoBehaviour
{
    public Vector3 heldOffset = new Vector3(0.5f, 1, 2);
    public Quaternion heldOffsetRotation;
    public bool isSnapped = false;
    public string objective = "";

    public AudioClip grabSound, dropSound, snapSound;
}
