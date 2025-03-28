using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] KeyCode[] keyToClose;
    MenuManager menuManager;
    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    void Update()
    {
        foreach (KeyCode key in keyToClose)
        {
            if (Input.GetKeyDown(key))
            {
                Close();
            }
        }
    }

    public virtual void Close()
    {
        menuManager.HideUI(false);
        menuManager.SetGamePause(false);
        gameObject.SetActive(false);
    }
}
