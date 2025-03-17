using UnityEngine;
using UnityEngine.UI;

public class Polaroid : MonoBehaviour
{
    [SerializeField] GameObject polaroidMenu;
    [SerializeField] public Texture textureToSet;
    MenuManager menuManager;
    InventoryManager inventoryManager;
    PlayerActions actions;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        actions = FindAnyObjectByType<PlayerActions>();
    }

    public void StartInteraction()
    {
        menuManager.SetGamePause(true);

        polaroidMenu.SetActive(true);
        polaroidMenu.GetComponentInChildren<RawImage>().texture = textureToSet;

        inventoryManager.AddPolaroid(gameObject);

        gameObject.SetActive(false);
        actions.interaction = null;
    }
}
