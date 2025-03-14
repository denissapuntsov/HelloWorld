using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
