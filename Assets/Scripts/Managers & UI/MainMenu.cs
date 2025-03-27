using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings Menu")]
    [SerializeField] private Button settingsBackButton;


    [Header("Scene References")]
    [SerializeField] private string gameSceneName = "HouseInteractable"; // The name of your game's first scene

    private void Start()
    {
        // Set up button click listeners
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        settingsBackButton.onClick.AddListener(CloseSettings);

        // Make sure the main menu is active and other panels are hidden
        ShowMainMenu();
    }



    // BUTTON FUNCTIONS
    private void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }

    private void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

  
  

    private void QuitGame()
    {
        // This works in builds but not in the Unity editor
        Application.Quit();
        // This will help with debugging in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        // Stop credits video if playing

    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits Scene");
    }
}