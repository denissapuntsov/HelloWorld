using UnityEditor;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    Animator crosshairAnimator;

    private void Start()
    {
        crosshairAnimator = GetComponent<Animator>();
    }

    public void SetCrosshairMode(string mode)
    {
        switch (mode)
        {
            case "idle":
                crosshairAnimator.SetInteger("state", 0);
                break;
            case "clear":
                crosshairAnimator.SetInteger("state", 1);
                break;
            case "wipe":
                crosshairAnimator.SetInteger("state", 2);
                break;
            case "pickUp":
                crosshairAnimator.SetInteger("state", 3);
                break;
            case "phone":
                crosshairAnimator.SetInteger("state", 4);
                break;
        }
    }
}
