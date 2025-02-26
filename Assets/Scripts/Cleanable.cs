using UnityEngine;

public class Cleanable : MonoBehaviour
{
    public void Clean()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }
}
