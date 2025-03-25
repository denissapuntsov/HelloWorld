using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectiveTracker, polaroidTracker;
    [SerializeField] List<string> objectiveList;

    int polaroidsCollected = 0;

    private void Start()
    {
        objectiveTracker.text = "";
        UpdateObjectiveSystem();
    }

    public void UpdateObjectiveSystem()
    {
        if (objectiveList.Count <= 0)
        {
            objectiveTracker.text = "";
        }

        foreach (string objective in objectiveList)
        {
            objectiveTracker.text += $"- {objective}\n";
        }
    }

    public void AddObjective(string newObjective)
    {
        objectiveTracker.text = "";
        objectiveList.Add(newObjective);
        UpdateObjectiveSystem();
    }

    public void RemoveObjective(string objectiveToRemove)
    {
        if (!objectiveList.Contains(objectiveToRemove)) { return; }
        objectiveTracker.text = "";
        objectiveList.Remove(objectiveToRemove);
        UpdateObjectiveSystem();
    }

    public void AddPolaroid()
    {
        if (polaroidsCollected == 0)
        {
            polaroidTracker.gameObject.SetActive(true);
        }

        polaroidsCollected++;

        polaroidTracker.text = $"Polaroids collected: {polaroidsCollected}/7";
    }

}
