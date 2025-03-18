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
        if (other.CompareTag("Feet"))
        {
            SetPlayerSpeed(normalSpeed + moveSpeedModifier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Feet"))
        {
            SetPlayerSpeed(normalSpeed);
        }
    }

    public void DestroyFirstChild()
    {
        Transform[] childArray = GetComponentsInChildren<Transform>();
        if (childArray.Length != 0)
        {
            Destroy(childArray[childArray.Length - 1].gameObject);
        }
        CheckForChildren();
    }

    private void CheckForChildren()
    {
        scoreManager.AddPoints(1);
        if (GetComponentsInChildren<Transform>().Length != 0) { return; }
        SetPlayerSpeed(normalSpeed);
        Destroy(gameObject);
    }

    void SetPlayerSpeed(float speed)
    {
         playerController.MoveSpeed = speed;
    }

}
