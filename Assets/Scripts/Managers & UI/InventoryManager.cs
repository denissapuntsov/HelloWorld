using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> polaroids;
    [SerializeField] GameObject inventoryUI, inventoryText, leftArrow, rightArrow, inventoryIconUI;
    [SerializeField] public int currentPolaroidIndex;

    public void AddPolaroid(GameObject polaroidToAdd)
    {
        inventoryIconUI.SetActive(true);
        polaroids.Add(polaroidToAdd);
    }

    public void DisplayPolaroid(int index)
    {
        currentPolaroidIndex = index;
        var displayedPolaroid = polaroids[index].GetComponent<Polaroid>();
        inventoryUI.GetComponentInChildren<RawImage>().texture = displayedPolaroid.textureToSet;
        inventoryText.GetComponent<TextMeshProUGUI>().text = $"\"{displayedPolaroid.displayString}\"";
        
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);

        if (currentPolaroidIndex == 0)
        {
            rightArrow.SetActive(false);
        }
        if (currentPolaroidIndex == polaroids.Count - 1) 
        { 
            leftArrow.SetActive(false); 
        }
    }
}
