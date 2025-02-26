using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    MenuManager menuManager;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    private void Update()
    {
        Close();
    }

    private void Close()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuManager.SetActivePause(false);
            this.gameObject.SetActive(false);
        }
    }
}
