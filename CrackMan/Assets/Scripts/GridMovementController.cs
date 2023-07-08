using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridMovementController : MonoBehaviour
{
    public float speed = 5f;
    public float epsilon = 0.01f;
    Vector3? _destination;
    
    public GridMovementManager.GridMovementDelegate onMovementStart;
    public GridMovementManager.GridMovementDelegate onMovementComplete;
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
        GridMovementManager.Instance.onGridMovementStart += () => onMovementStart.Invoke();
    }

    [Button]
    public void InvokeMovementStart()
    {
        onMovementStart.Invoke();
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

}
