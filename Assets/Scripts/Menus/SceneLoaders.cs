using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneLoaders : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private VideoPlayer creditsVideo;
    void Start()
    {
        creditsVideo.loopPointReached += BacktoMenu;
    }

    // Update is called once per frame
    void BacktoMenu(VideoPlayer vpo)
    {
        SceneManager.LoadScene(1);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(2);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(1);
    }
}
