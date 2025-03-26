using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToCredits : MonoBehaviour
{
    public void GoToCredits()
    {
        SceneManager.LoadScene("credits");
        Debug.Log("loading credits");
    }
}
