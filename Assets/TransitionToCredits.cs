using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToCredits : MonoBehaviour
{
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits Scene");
        Debug.Log("loading credits");
    }
}
