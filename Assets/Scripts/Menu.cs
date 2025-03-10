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
                Debug.Log("got close key");
                Close();
            }
        }
    }

    protected virtual void Close()
    {
        menuManager.SetGamePause(false);
        gameObject.SetActive(false);
    }
}
