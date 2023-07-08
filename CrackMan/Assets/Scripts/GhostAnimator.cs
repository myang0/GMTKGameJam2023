using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimator : MonoBehaviour
{
    public Animator animator;

    public void SetFacing(GridMovementController.Direction dir)
    {
        switch(dir)
        {
            case GridMovementController.Direction.Left:
                animator.SetTrigger("FaceLeft");
                break;
            case GridMovementController.Direction.Right:
                animator.SetTrigger("FaceRight");
                break;
            case GridMovementController.Direction.Up:
                animator.SetTrigger("FaceUp");
                break;
            case GridMovementController.Direction.Down:
                animator.SetTrigger("FaceDown");
                break;
        }
    }

}
