using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public GridMovementController.Direction facing;
    public GridMovementController.Direction originalFacing;

    public GhostAnimator ghostAnimator;

    public void BaseStart()
    {
        originalFacing = facing;
    }
    
    public void SetNewFacing(GridMovementController.Direction newDirection)
    {
        facing = newDirection;
        originalFacing = newDirection;

        ghostAnimator.SetFacing(newDirection);
    }
}
