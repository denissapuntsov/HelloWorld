using System.Collections;
using UnityEngine;

public class StartTutorialPanel : Menu
{
    [SerializeField] int timeUntilCanClose = 4;
    [SerializeField] GameObject canBeClosedUIText;
    MenuManager menuManager;
    bool canBeClosed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
        menuManager.SetGamePause(true);
        menuManager.SetPlayerMovement(false);
        menuManager.HideUI(true);
        StartCoroutine(DelayWindowClose());
    }

    public override void Close()
    {
        if (canBeClosed)
        {
            menuManager.SetGamePause(false);
            menuManager.SetPlayerMovement(true);
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
