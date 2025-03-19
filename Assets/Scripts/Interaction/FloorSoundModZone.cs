using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Xml;

public enum FloorType
{
    floor,
    rug,
    tile
};

[RequireComponent(typeof(BoxCollider))]
public class FloorSoundModZone : MonoBehaviour
{
    public FloorType floorType = new FloorType();

    Footsteps footsteps;

    private void Start()
    {
        footsteps = FindAnyObjectByType<Footsteps>();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Feet")) { return; }
        footsteps.SetActiveClipPool((int)floorType);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Feet")) { return; }
        footsteps.SetActiveClipPool(0);
    }
}
