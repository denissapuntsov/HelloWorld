using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SpeedModArea : MonoBehaviour
{
    [SerializeField] float moveSpeedModifier = 0.5f;
    FirstPersonController playerController;
    ScoreManager scoreManager;
    float normalSpeed;

    private void Start()
    {
        playerController = FindAnyObjectByType<FirstPersonController>();
        normalSpeed = playerController.MoveSpeed;
        scoreManager = FindAnyObjectByType<ScoreManager>();
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
        scoreManager.AddPoints(1);
        if (GetComponentsInChildren<Interaction>().Length != 0) { return; }
        SetPlayerSpeed(normalSpeed);
        Destroy(gameObject);
    }

    void SetPlayerSpeed(float speed)
    {
         playerController.MoveSpeed = speed;
    }

}
