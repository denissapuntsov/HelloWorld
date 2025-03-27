using UnityEngine;
using UnityEngine.UI;

public class Polaroid : MonoBehaviour
{
    [SerializeField] GameObject polaroidMenu;
    [SerializeField] public Texture textureToSet;
    [SerializeField] public Holdable relatedHoldable;
    [SerializeField] public string displayString;
    [SerializeField] private int pointValue = 2;
    [SerializeField] AudioClip polaroidPickUp;
    [SerializeField] AudioSource globalSFXAudioSource;

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
        // add points to score
        var scoreManager = FindAnyObjectByType<ScoreManager>();
        scoreManager.AddPoints(pointValue);
        if (scoreManager.score == 100) 
        {
            polaroidMenu.SetActive(false);
            return; 
        }

        globalSFXAudioSource.PlayOneShot(polaroidPickUp);

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
