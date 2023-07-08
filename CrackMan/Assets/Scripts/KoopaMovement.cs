using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KoopaMovement : MonoBehaviour
{
    Tilemap _tilemap;
    public string wallTileName = "Textures-16_51";
    
    public GridMovementController gridMovementController;
    public GridMovementController.Direction facing;

    GridMovementController.Direction _originalFacing;

    // Start is called before the first frame update
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
        // turn around if we hit a wall
        TileBase aheadTile = gridMovementController.GetAdjacentTile(_tilemap, facing);
        if (aheadTile && aheadTile.name == wallTileName)
        {
            facing = GetOppositeDirection(facing);
        }
        gridMovementController.GoAdjacent(facing);
    }
    
    void HandleMovementComplete()
    {
    }

    void HandleMovementReset()
    {
        facing = _originalFacing;
    }

    GridMovementController.Direction GetOppositeDirection(GridMovementController.Direction dir)
    {
        switch (dir)
        {
            case GridMovementController.Direction.Left:
                return GridMovementController.Direction.Right;
            case GridMovementController.Direction.Right:
                return GridMovementController.Direction.Left;
            case GridMovementController.Direction.Up:
                return GridMovementController.Direction.Down;
            case GridMovementController.Direction.Down:
                return GridMovementController.Direction.Up;
            default:
                return GridMovementController.Direction.Left;
        }
    }
}
