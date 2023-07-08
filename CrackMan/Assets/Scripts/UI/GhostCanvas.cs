using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostCanvas : MonoBehaviour
{
    public Sprite normalArrowSprite;
    public Sprite highlightedArrowSprite;

    public GameObject arrowParent;
    public List<Image> arrowImages;

    public GhostMovement ghostMovement;

    bool _playing = false;

    Dictionary<int, GridMovementController.Direction> _indexToDirectionMap; 

    void Start()
    {
        _indexToDirectionMap = new Dictionary<int, GridMovementController.Direction>();
        _indexToDirectionMap.Add(0, GridMovementController.Direction.Up);
        _indexToDirectionMap.Add(1, GridMovementController.Direction.Down);
        _indexToDirectionMap.Add(2, GridMovementController.Direction.Left);
        _indexToDirectionMap.Add(3, GridMovementController.Direction.Right);

        GridMovementManager.Instance.onGridMovementStart += HandleMovementStart;
        GridMovementManager.Instance.onGridReset += HandleGridReset;
    }

    void HandleMovementStart()
    {
        _playing = true;
    }

    void HandleGridReset()
    {
        _playing = false;
    }

    public void OnArrowClick(int index)
    {
        if (index > arrowImages.Count || index < 0)
            return;

        foreach (Image arrowImage in arrowImages)
        {
            arrowImage.sprite = normalArrowSprite;
        }
        arrowImages[index].sprite = highlightedArrowSprite;

        ghostMovement.SetNewFacing(_indexToDirectionMap[index]);
    }

    void OnMouseOver()
    {
        if (_playing)
            return;
        
        arrowParent.SetActive(true);
    }

    void OnMouseExit()
    {
        arrowParent.SetActive(false);
    }

    void OnDestroy()
    {
        if (GridMovementManager.Instance.onGridMovementStart != null)
            GridMovementManager.Instance.onGridMovementStart -= HandleMovementStart;

        if (GridMovementManager.Instance.onGridReset != null)
            GridMovementManager.Instance.onGridReset -= HandleGridReset;
    }
}
