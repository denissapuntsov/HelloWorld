using UnityEngine;
using UnityEngine.UI;

public class Polaroid : MonoBehaviour
{
    [SerializeField] GameObject polaroidMenu;
    [SerializeField] public Texture textureToSet;
    [SerializeField] public Holdable relatedHoldable;
    [SerializeField] public string displayString;
    MenuManager menuManager;
    ObjectiveManager objectiveManager;
    InventoryManager inventoryManager;
    AudioManager audioManager;
    PlayerActions actions;

    private void Start()
    {
        menuManager = FindAnyObjectByType<MenuManager>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        objectiveManager = FindAnyObjectByType<ObjectiveManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        actions = FindAnyObjectByType<PlayerActions>();
    }

    public void StartInteraction()
    {
        menuManager.SetGamePause(true);
        audioManager.PauseAudio(false);
        Time.timeScale = 1;

        polaroidMenu.SetActive(true);
        polaroidMenu.GetComponentInChildren<RawImage>().texture = textureToSet;
        inventoryManager.AddPolaroid(gameObject);

        gameObject.SetActive(false);
        actions.interaction = null;
        
        //update objective lists
        if (!relatedHoldable.isSnapped)
        {
            objectiveManager.AddObjective(relatedHoldable.objective);
        }
        objectiveManager.AddPolaroid();
    }
}
