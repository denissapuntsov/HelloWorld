using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings Menu")]
    [SerializeField] private Button settingsBackButton;

    [Header("Credits Menu")]
    [SerializeField] private Button creditsBackButton;

    [Header("Credits Video")]
    [SerializeField] private VideoPlayer creditsVideoPlayer;
    [SerializeField] private RawImage creditsVideoDisplay;
    [SerializeField] private float creditsVideoDelay = 0.5f;
    [SerializeField] private Button skipVideoButton; // Optional: button to skip the video

    [Header("Scene References")]
    [SerializeField] private string gameSceneName = "HouseInteractable"; // The name of your game's first scene

    private void Start()
    {
        // Set up button click listeners
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        quitButton.onClick.AddListener(QuitGame);
        settingsBackButton.onClick.AddListener(CloseSettings);
        creditsBackButton.onClick.AddListener(CloseCredits);

        // Set up video player if present
        if (creditsVideoPlayer != null)
        {
            SetupCreditsVideo();

            // Add skip video button functionality if provided
            if (skipVideoButton != null)
            {
                skipVideoButton.onClick.AddListener(SkipCreditsVideo);
            }
        }

        // Make sure the main menu is active and other panels are hidden
        ShowMainMenu();
    }

    private void SetupCreditsVideo()
    {
        // Setup video player callback for when video is done
        creditsVideoPlayer.loopPointReached += OnCreditsVideoEnd;

        // Ensure video isn't playing at start
        creditsVideoPlayer.Stop();

        // Connect the video player to the display texture if available
        if (creditsVideoDisplay != null && creditsVideoPlayer.clip != null)
        {
            // Create a render texture with the correct aspect ratio
            RenderTexture renderTexture = new RenderTexture(
                (int)creditsVideoPlayer.clip.width,
                (int)creditsVideoPlayer.clip.height,
                24);

            renderTexture.Create();
            creditsVideoPlayer.targetTexture = renderTexture;
            creditsVideoDisplay.texture = renderTexture;
        }
    }

    private void OnCreditsVideoEnd(VideoPlayer vp)
    {
        // Video ended, you can add logic here if needed
        // For example, auto-return to main menu or show additional content
        Debug.Log("Credits video playback completed");

        // Optionally show back button if it was hidden during playback
        if (creditsBackButton != null && !creditsBackButton.gameObject.activeSelf)
        {
            creditsBackButton.gameObject.SetActive(true);
        }

        // Hide skip button if it exists
        if (skipVideoButton != null)
        {
            skipVideoButton.gameObject.SetActive(false);
        }
    }

    private void PlayCreditsVideo()
    {
        if (creditsVideoPlayer != null && creditsVideoPlayer.clip != null)
        {
            // Optionally hide back button during video
            // if (creditsBackButton != null)
            // {
            //     creditsBackButton.gameObject.SetActive(false);
            // }

            // Show skip button if it exists
            if (skipVideoButton != null)
            {
                skipVideoButton.gameObject.SetActive(true);
            }

            // Play the video
            creditsVideoPlayer.Play();
        }
    }

    public void SkipCreditsVideo()
    {
        if (creditsVideoPlayer != null && creditsVideoPlayer.isPlaying)
        {
            creditsVideoPlayer.Stop();

            // Show back button if it was hidden
            if (creditsBackButton != null && !creditsBackButton.gameObject.activeSelf)
            {
                creditsBackButton.gameObject.SetActive(true);
            }

            // Hide skip button
            if (skipVideoButton != null)
            {
                skipVideoButton.gameObject.SetActive(false);
            }
        }
    }

    // BUTTON FUNCTIONS
    private void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("HouseInteractable");
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

    private void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        // Play credits video after a short delay
        if (creditsVideoPlayer != null)
        {
            Invoke("PlayCreditsVideo", creditsVideoDelay);
        }
    }

    private void CloseCredits()
    {
        // Stop the video if it's playing
        if (creditsVideoPlayer != null && creditsVideoPlayer.isPlaying)
        {
            creditsVideoPlayer.Stop();
        }

        creditsPanel.SetActive(false);
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
        creditsPanel.SetActive(false);

        // Stop credits video if playing
        if (creditsVideoPlayer != null && creditsVideoPlayer.isPlaying)
        {
            creditsVideoPlayer.Stop();
        }
    }
}