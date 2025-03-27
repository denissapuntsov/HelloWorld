using StarterAssets;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    // for pausing all movement in game
    [SerializeField] FirstPersonController playerController;
    PlayerActions actions;
    InventoryManager inventoryManager;
    AudioManager audioManager;

    private bool isInMenu = false;
    public bool gameHasFinished = false;
    public bool isPlayerFrozenExternally = false;
    public bool isPaused;

    // Menu references
    [SerializeField] public GameObject pauseUI, inventoryUI, UIToHide;

    void Start()
    {
        actions = FindAnyObjectByType<PlayerActions>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Update()
    {
        if (!isInMenu && !gameHasFinished)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideUI(true);
                pauseUI.SetActive(true);
                SetGamePause(true);
            }
            if (Input.GetKeyDown(KeyCode.Tab) && inventoryManager.polaroids.Count != 0)
            {
                HideUI(true);
                inventoryUI.SetActive(true);
                inventoryManager.DisplayPolaroid(0);
                SetGamePause(true);
            }
        }
    }

    public void SetGamePause(bool state)
    {
        isInMenu = state;
        isPaused = state;

        audioManager.PauseAudio(state);

        //Make sure the player's movements are not unfrozen when entering pause menu from telephone call
        if (!isPlayerFrozenExternally) { SetPlayerMovement(!state); }

        //Freeze time
        Time.timeScale = state ? 0.0f : 1.0f;
        FindAnyObjectByType<ScoreManager>().timerPaused = state;

        // Set Cursor parameters
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    public void SetPlayerMovement(bool state)
    {
        playerController.enabled = state;
        actions.canInteract = state;
    }

    public void HideUI(bool state)
    {
        UIToHide.SetActive(!state);
    }
}
