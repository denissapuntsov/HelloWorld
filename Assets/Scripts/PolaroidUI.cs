using UnityEngine;

public class PolaroidUI : Menu
{
    protected override void Close()
    {
        base.Close();


        Destroy(gameObject);
    }
}
