using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectiveSystem;
    [SerializeField] List<string> objectiveList;

    private void Start()
    {
        objectiveSystem.text = "";
        UpdateObjectiveSystem();
    }

    public void UpdateObjectiveSystem()
    {
        if (objectiveList.Count <= 0)
        {
            objectiveSystem.text = "";
        }

        foreach (string objective in objectiveList)
        {
            objectiveSystem.text += $"- {objective}\n";
        }
    }

    public void AddObjective(string newObjective)
    {
        objectiveList.Add(newObjective);
        UpdateObjectiveSystem();
    }

    public void RemoveObjective(string objectiveToRemove)
    {
        if (!objectiveList.Contains(objectiveToRemove)) 
        { 
            Debug.LogError($"No objective {objectiveToRemove} found in objective list."); 
            return; 
        }

        objectiveList.Remove(objectiveToRemove);
        UpdateObjectiveSystem();
    }


}
