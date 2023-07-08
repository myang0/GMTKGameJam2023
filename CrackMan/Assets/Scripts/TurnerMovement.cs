using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnerMovement : MonoBehaviour
{
    public GridMovementController gridMovementController;
    
    public bool isLeftTurner = true;

    Tilemap _tilemap;
    public string wallTileName = "Textures-16_51";

    public GridMovementController.Direction facing;
    GridMovementController.Direction _originalFacing;
    void Start()
    {
        _originalFacing = facing;
        _tilemap = GameObject.FindWithTag("WallTilemap").GetComponent<Tilemap>();
        gridMovementController.onMovementStart += HandleMovementStart;
        gridMovementController.onMovementComplete += HandleMovementComplete;
        gridMovementController.onMovementReset += HandleMovementReset;
    }

    void HandleMovementStart()
    {
        // turn left or right if we hit a wall, up to 3 times
        for (int i = 0; i < 3; i++)
        {
            TileBase aheadTile = gridMovementController.GetAdjacentTile(_tilemap, facing);
            if (aheadTile && aheadTile.name == wallTileName)
            {
                facing = isLeftTurner ? GetLeftDirection(facing) : GetRightDirection(facing);
            }
            else
            {
                gridMovementController.GoAdjacent(facing);
                return;
            }
        }
        Debug.LogError("Turner is stuck!");
    }
    
    void HandleMovementComplete()
    {
    }
    
    void HandleMovementReset()
    {
        facing = _originalFacing;
    }
    
    GridMovementController.Direction GetLeftDirection(GridMovementController.Direction dir)
    {
        switch (dir)
        {
            case GridMovementController.Direction.Left:
                return GridMovementController.Direction.Down;
            case GridMovementController.Direction.Right:
                return GridMovementController.Direction.Up;
            case GridMovementController.Direction.Up:
                return GridMovementController.Direction.Left;
            case GridMovementController.Direction.Down:
                return GridMovementController.Direction.Right;
            default:
                return GridMovementController.Direction.Left;
        }
    }
    
    GridMovementController.Direction GetRightDirection(GridMovementController.Direction dir)
    {
        switch (dir)
        {
            case GridMovementController.Direction.Left:
                return GridMovementController.Direction.Up;
            case GridMovementController.Direction.Right:
                return GridMovementController.Direction.Down;
            case GridMovementController.Direction.Up:
                return GridMovementController.Direction.Right;
            case GridMovementController.Direction.Down:
                return GridMovementController.Direction.Left;
            default:
                return GridMovementController.Direction.Right;
        }
    }


}