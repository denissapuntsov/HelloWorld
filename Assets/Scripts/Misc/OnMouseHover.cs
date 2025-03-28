using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnMouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] int siblingIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        try
        {
            transform.SetSiblingIndex(siblingIndex);
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e + " Invalid sibling index.");
            throw;
        }
    }
}
