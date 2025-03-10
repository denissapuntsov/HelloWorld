using Mono.Cecil.Cil;
using StarterAssets;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    // for pausing all movement in game
    [SerializeField] FirstPersonController playerController;
    PlayerActions actions;

    //
    public bool isInMenu = false;
    public bool isPaused;

    // Menu references
    [SerializeField] GameObject pause, inventory;

    void Start()
    {
        actions = FindAnyObjectByType<PlayerActions>();
    }

    private void Update()
    {
        if (isInMenu) { return; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(true);
            SetGamePause(true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);
            SetGamePause(true);
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
