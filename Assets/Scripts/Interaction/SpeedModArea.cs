using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class SpeedModArea : MonoBehaviour
{
    [SerializeField] float moveSpeedModifier = 0.5f;
    [SerializeField] GameObject sparkleParticles;
    FirstPersonController playerController;
    ScoreManager scoreManager;
    float normalSpeed;

    public UnityEvent onClean;

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
        if (transform.childCount >= 0)
        {
            Instantiate(sparkleParticles, transform.GetChild(0).position, transform.GetChild(0).rotation, transform.parent);
            Destroy(transform.GetChild(0).gameObject);

            if (transform.childCount == 1)
            {
                onClean?.Invoke();
                SetPlayerSpeed(normalSpeed);
                Destroy(gameObject);
            }
        }

        scoreManager.AddPoints(1);
    }

    void SetPlayerSpeed(float speed)
    {
         playerController.MoveSpeed = speed;
    }

}
