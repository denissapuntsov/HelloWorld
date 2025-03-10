using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> polaroids;
    [SerializeField] GameObject inventoryUI, leftArrow, rightArrow;
    [SerializeField] public int currentPolaroidIndex;

    public void AddPolaroid(GameObject polaroidToAdd)
    {
        polaroids.Add(polaroidToAdd);
    }

    public void DisplayPolaroid(int index)
    {
        currentPolaroidIndex = index;
        inventoryUI.GetComponentInChildren<RawImage>().texture = polaroids[index].GetComponent<Polaroid>().textureToSet;
        
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
