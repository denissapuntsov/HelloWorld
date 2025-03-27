using System.Collections;
using UnityEngine;

public class TutorialPanel : Menu
{
    [SerializeField] int timeUntilCanClose = 2;
    [SerializeField] GameObject canBeClosedUIText;
    [SerializeField] MusicManager musicManager;
    MenuManager menuManager;
    bool canBeClosed = false;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
        menuManager.SetGamePause(true);
        Time.timeScale = 1;
        menuManager.HideUI(true);
        StartCoroutine(DelayWindowClose());
    }

    public override void Close()
    {
        if (canBeClosed)
        {
            musicManager.PlayMusic();
            menuManager.SetGamePause(false);
            menuManager.HideUI(false);
            gameObject.SetActive(false);
        }
    }

    IEnumerator DelayWindowClose()
    {
        yield return new WaitForSecondsRealtime(timeUntilCanClose);
        canBeClosedUIText.SetActive(true);
        canBeClosed = true;
    }
}
