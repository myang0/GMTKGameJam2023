using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementManager : Singleton<GridMovementManager>
{
    public Vector2Int gridSize = new Vector2Int(1, 1);
    public Vector2Int gridOffset = new Vector2Int(0, 0);

    public float tickTime = 1f;
    [SerializeField] float _tickTimeInternal = 0f;

    // event to emit to notify that a grid movement has occurred
    public delegate void GridMovementDelegate();
    public GridMovementDelegate onGridMovementStart;

    void Start()
    {
        onGridMovementStart += () => Debug.Log("Grid Movement Ticked");
    }

    void FixedUpdate()
    {
        _tickTimeInternal += Time.deltaTime;
        if (_tickTimeInternal >= tickTime)
        {
            _tickTimeInternal = 0f;
            onGridMovementStart?.Invoke();
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 pos = new Vector3(
                    x * gridSize.x + gridOffset.x,
                    y * gridSize.y + gridOffset.y,
                    0
                );
                Gizmos.DrawWireCube(pos, new Vector3(gridSize.x, gridSize.y, 0));
            }
        }
    }

}
