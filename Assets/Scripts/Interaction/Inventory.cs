using UnityEngine;

public class Inventory : MonoBehaviour
{
    InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    private void Update()
    {
        Navigate();
    }

    private void Navigate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (inventoryManager.currentPolaroidIndex == inventoryManager.polaroids.Count - 1) { Debug.Log("Nowhere to scroll"); return; }
            DisplayNext();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (inventoryManager.currentPolaroidIndex == 0) { Debug.Log("Nowhere to scroll"); return; }
            DisplayPrevious();
        }
    }

    public void DisplayPrevious()
    {
        inventoryManager.DisplayPolaroid(inventoryManager.currentPolaroidIndex - 1);
    }

    public void DisplayNext()
    {
        inventoryManager.DisplayPolaroid(inventoryManager.currentPolaroidIndex + 1);
    }
}
