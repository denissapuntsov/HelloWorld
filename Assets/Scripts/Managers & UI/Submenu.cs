using UnityEngine;

public class Submenu : Menu
{
    [SerializeField] GameObject parentMenu;

    public override void Close()
    {
        parentMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
