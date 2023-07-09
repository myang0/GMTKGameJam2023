using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackmanAnimator : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetFacing(GridMovementController.Direction dir)
    {
        switch(dir)
        {
            case GridMovementController.Direction.Left:
                animator.SetTrigger("TurnLeft");
                break;
            case GridMovementController.Direction.Right:
                animator.SetTrigger("TurnRight");
                break;
            case GridMovementController.Direction.Up:
                animator.SetTrigger("TurnUp");
                break;
            case GridMovementController.Direction.Down:
                animator.SetTrigger("TurnDown");
                break;
        }
    }

}
