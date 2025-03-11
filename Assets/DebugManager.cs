using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    void Update()
    {
        ReloadScene();
    }

    void ReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            SceneManager.LoadScene(0);
        }
    }
}
