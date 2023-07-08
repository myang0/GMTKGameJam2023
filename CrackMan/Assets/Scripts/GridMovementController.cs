using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovementController : MonoBehaviour
{
    public float speed = 5f;
    public float epsilon = 0.01f;
    Vector3? _destination;

    Vector3 _originalPosition;
    
    public GridMovementManager.GridMovementDelegate onMovementStart;
    public GridMovementManager.GridMovementDelegate onMovementComplete;
    public GridMovementManager.GridMovementDelegate onMovementReset;
    
    void Start()
    {
        if (GridMovementManager.Instance == null)
        {
            Debug.LogError("GridMovementManager not found in scene");
            return;
        }
        if (GridMovementManager.Instance.onGridMovementStart == null)
        {
            Debug.LogError("GridMovementManager.onGridMovementStart is null");
            return;
        }
        if (GridMovementManager.Instance.onGridReset == null)
        {
            Debug.LogError("GridMovementManager.onGridReset is null");
            return;
        }

        _originalPosition = transform.position;

        GridMovementManager.Instance.onGridMovementStart += HandleMovementStart;
        GridMovementManager.Instance.onGridReset += HandleGridReset;
    }

    void HandleMovementStart()
    {
        onMovementStart.Invoke();
    }

    void HandleGridReset()
    {
        transform.position = _originalPosition;
        _destination = null;
        onMovementReset.Invoke();
    }

    [Button]
    public void InvokeMovementStart()
    {
        onMovementStart.Invoke();
    }

    public void SetNewOriginalPosition(Vector3 newOriginalPosition)
    {
        _originalPosition = newOriginalPosition;
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    void FixedUpdate()
    {
        if (_destination == null)
        {
            return;
        }
        
        if (Vector3.Distance(transform.position, (Vector3)_destination) < epsilon)
        {
            _destination = null;
            onMovementComplete.Invoke();
            return;
        }
        
        transform.position = Vector3.MoveTowards(
            transform.position, 
            (Vector3)_destination,
            speed * Time.deltaTime
        );
    }

    [Button]
    public void GoAdjacent(Direction dir)
    {
        _destination = GetAdjacentPosition(dir);
    }

    public Vector3 GetAdjacentPosition(Direction dir)
    {
        Vector3 destination = dir switch {
            Direction.Left => transform.position + Vector3.left * GridMovementManager.Instance.gridSize.x,
            Direction.Right => transform.position + Vector3.right * GridMovementManager.Instance.gridSize.x,
            Direction.Up => transform.position + Vector3.up * GridMovementManager.Instance.gridSize.y,
            Direction.Down => transform.position + Vector3.down * GridMovementManager.Instance.gridSize.y,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
        return destination;
    }
    
    public TileBase GetAdjacentTile(Tilemap tilemap, Direction dir)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(GetAdjacentPosition(dir));
        return tilemap.GetTile(cellPosition);
    }

    void OnDestroy()
    {
        if (GridMovementManager.Instance.onGridMovementStart != null)
            GridMovementManager.Instance.onGridMovementStart -= HandleMovementStart;

        if (GridMovementManager.Instance.onGridReset != null)
            GridMovementManager.Instance.onGridReset -= HandleGridReset;
    }
}
