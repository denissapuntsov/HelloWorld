using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpeedModArea : MonoBehaviour
{
    [SerializeField] float moveSpeedModifier = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("got player");
            other.GetComponent<FirstPersonController>().MoveSpeed += moveSpeedModifier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            other.GetComponent<FirstPersonController>().MoveSpeed -= moveSpeedModifier;
        }
    }

}
