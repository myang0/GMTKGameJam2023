using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KoopaMovement : GhostMovement
{
    Tilemap _tilemap;
    public string wallTileName = "Textures-16_51";
    public string collisionTag;
    public float collisionRadius = 0.1f;
    public GridMovementController gridMovementController;

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
        // turn around if we hit a wall
        for (int i = 0; i < 2; i++)
        {
            TileBase aheadTile = gridMovementController.GetAdjacentTile(_tilemap, facing);
            bool facingWall = aheadTile && aheadTile.name == wallTileName;
            
            Vector3 aheadPosition = gridMovementController.GetAdjacentPosition(facing);
            bool facingCollision = Physics2D.OverlapCircle(aheadPosition, collisionRadius, LayerMask.GetMask(collisionTag));
            if (facingWall || facingCollision)
            {
                var opposite = GetOppositeDirection(facing);
                SetFacing(opposite);
            }
            else
            {
                gridMovementController.GoAdjacent(facing);
                return;
            }
        }
        Debug.LogError("Koopa is stuck!");
    }
    
    void HandleMovementComplete()
    {
    }

    void HandleMovementReset()
    {
        SetFacing(originalFacing);
    }
    
    [Button]
    public void PrintAheadTile()
    {
        TileBase aheadTile = gridMovementController.GetAdjacentTile(_tilemap, facing);
        Debug.Log(aheadTile.name);
    }

    void SetFacing(GridMovementController.Direction dir)
    {
        facing = dir;
        ghostAnimator.SetFacing(dir);
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
