using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KoopaMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public string wallTileName = "Textures-16_51";
    
    public GridMovementController gridMovementController;
    public GridMovementController.Direction facing;

    GridMovementController.Direction _originalFacing;

    // Start is called before the first frame update
    void Start()
    {
        if (tilemap == null)
        {
            GameObject tilemapObject = GameObject.Find("Tilemap");
            tilemap = tilemapObject.GetComponent<Tilemap>();
        }

        _originalFacing = facing;

        gridMovementController.onMovementStart += HandleMovementStart;
        gridMovementController.onMovementComplete += HandleMovementComplete;
        gridMovementController.onMovementReset += HandleMovementReset;
    }
    
    void HandleMovementStart()
    {
        gridMovementController.GoAdjacent(facing);
    }
    
    void HandleMovementComplete()
    { 
        // turn around if we hit a wall
        Vector3 aheadTilePos = gridMovementController.GetAdjacentPosition(facing);
        Vector3Int aheadCellPos = tilemap.WorldToCell(aheadTilePos);
        TileBase aheadTile = tilemap.GetTile(aheadCellPos);
        if (aheadTile && aheadTile.name == wallTileName)
        {
            facing = GetOppositeDirection(facing);
        }

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
