using StarterAssets;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    // for pausing all movement in game
    [SerializeField] FirstPersonController playerController;
    PlayerActions actions;
    InventoryManager inventoryManager;

    //
    public bool isInMenu = false;
    public bool isPaused;

    // Menu references
    [SerializeField] GameObject pauseUI, inventoryUI;

    void Start()
    {
        actions = FindAnyObjectByType<PlayerActions>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    private void Update()
    {
        if (!isInMenu)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseUI.SetActive(true);
                SetGamePause(true);
            }
            if (Input.GetKeyDown(KeyCode.I) && inventoryManager.polaroids.Count != 0)
            {
                inventoryUI.SetActive(true);
                inventoryManager.DisplayPolaroid(0);
                SetGamePause(true);
            }
        }
    }

    public void SetGamePause(bool state)
    {
        isInMenu = state;

        playerController.enabled = !state;
        actions.canInteract = !state;
        Time.timeScale = state ? 0.0f : 1.0f;
        isPaused = state;

        // Set Cursor parameters
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state; 
    }
}
