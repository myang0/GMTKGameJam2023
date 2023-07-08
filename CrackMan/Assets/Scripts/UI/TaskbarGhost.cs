using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TaskbarGhost : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool hoveredOver = false;
    public int remainingGhosts = 2;
    int _maxGhosts;

    public TMP_Text countText;

    public GameObject ghostPrefab;

    public void Initialize(int ghosts)
    {
        remainingGhosts = ghosts;
        _maxGhosts = remainingGhosts;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredOver = false;
    }

    public GameObject HandleDrag()
    {
        if (remainingGhosts == 0)
            return null;
        
        GameObject ghostObject = Instantiate(ghostPrefab, transform.position, Quaternion.identity);

        AddToGhostCount(-1);
        return ghostObject;
    }

    public void ReleaseGhost()
    {
        AddToGhostCount(1);
    }

    public void AddToGhostCount(int addend)
    {
        if (remainingGhosts + addend > _maxGhosts)
            return;
        
        remainingGhosts += addend;
        countText.text = $"{remainingGhosts}";
    }
}
