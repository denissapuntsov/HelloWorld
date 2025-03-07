using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpeedModArea : MonoBehaviour
{
    [SerializeField] float moveSpeedModifier = 0.5f;
    FirstPersonController playerController;
    float normalSpeed;

    private void Start()
    {
        playerController = FindAnyObjectByType<FirstPersonController>();
        normalSpeed = playerController.MoveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerSpeed(normalSpeed + moveSpeedModifier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerSpeed(normalSpeed);
         }
    }

    public void CheckForChildren()
    {
        if (GetComponentsInChildren<Interaction>().Length != 0) { return; }
        SetPlayerSpeed(normalSpeed);
        Destroy(gameObject);
    }

    void SetPlayerSpeed(float speed)
    {
         playerController.MoveSpeed = speed;
    }

}
