using Mono.Cecil.Cil;
using StarterAssets;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool isPaused = false;
    public bool isInMenu = false;
    public bool isInInventory = false;

    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] FirstPersonController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInInventory && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if (!isInMenu && Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory(true);
        }
        if (isInInventory && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory(false);
        }
    }

    private void TogglePauseMenu()
    {
        if (!isPaused && !isInInventory)
        {
            SetActivePause(true);
            isInMenu = true;
            pauseMenuCanvas.SetActive(true);
        }
        else
        {
            SetActivePause(false);
            isInMenu = false;
            pauseMenuCanvas.SetActive(false);
        }
    }

    public void SetActivePause(bool state)
    {
        playerController.enabled = !state;
        Time.timeScale = state ? 0.0f : 1.0f;
        isPaused = state;
    }

    void ToggleInventory(bool state)
    {
        if (state == true)
        {
            if (!isPaused && !isInInventory)
            {
                SetActivePause(true);
                isInInventory = true;
                inventoryCanvas.SetActive(true);
            }
        }
        if (state == false)
        {
            SetActivePause(false);
            isInInventory = false;
            inventoryCanvas.SetActive(false);
        }
    }
}
