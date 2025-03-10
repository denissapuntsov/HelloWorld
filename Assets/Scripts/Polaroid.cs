using UnityEngine;

public class Polaroid : MonoBehaviour
{
    [SerializeField] GameObject polaroidCanvas;
    MenuManager menuManager;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    public void StartInteraction()
    {
        menuManager.SetGamePause(true);
        Instantiate(polaroidCanvas, menuManager.transform);
        gameObject.SetActive(false);
    }
}
