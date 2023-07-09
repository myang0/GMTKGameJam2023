using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnerMovement : GhostMovement
{
    public GridMovementController gridMovementController;
    
    public bool isLeftTurner = true;

    Tilemap _tilemap;
    public string wallTileName = "Textures-16_51";
    
    public string collisionTag;
    public float collisionRadius = 0.1f;

    void Start()
    {
        _tilemap = GameObject.FindWithTag("WallTilemap").GetComponent<Tilemap>();
        gridMovementController.onMovementStart += HandleMovementStart;
        gridMovementController.onMovementComplete += HandleMovementComplete;
        gridMovementController.onMovementReset += HandleMovementReset;

        BaseStart();
    }

    void HandleMovementStart()
    {
        // turn left or right if we hit a wall, up to 3 times
        for (int i = 0; i < 3; i++)
        {
            TileBase aheadTile = gridMovementController.GetAdjacentTile(_tilemap, facing);
            bool facingWall = aheadTile && aheadTile.name == wallTileName;
            
            Vector3 aheadPosition = gridMovementController.GetAdjacentPosition(facing);
            bool facingCollision = Physics2D.OverlapCircle(aheadPosition, collisionRadius, LayerMask.GetMask(collisionTag));
            if (facingWall || facingCollision)
            {
                facing = isLeftTurner ? GetLeftDirection(facing) : GetRightDirection(facing);
                ghostAnimator.SetFacing(facing);
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
        facing = originalFacing;
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
