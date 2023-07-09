using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackmanMovement : MonoBehaviour
{
    public GridMovementController gridMovementController;
    public List<Waypoint> waypoints;
    int _currentWaypointIndex = 0;
    Waypoint CurrentWaypoint => waypoints[_currentWaypointIndex];
    public float epsilon = 0.01f;

    CrackmanAnimator _crackmanAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        _crackmanAnimator = GetComponent<CrackmanAnimator>();

        gridMovementController.onMovementStart += HandleMovementStart;
        gridMovementController.onMovementComplete += HandleMovementComplete;
        gridMovementController.onMovementReset += HandleMovementReset;
    }

    void HandleMovementStart()
    {
        if (_currentWaypointIndex >= waypoints.Count)
        {
            return;
        }
        MoveToWaypoint(CurrentWaypoint);
    }
    
    void HandleMovementComplete()
    { 
        if (IsAtWaypoint(CurrentWaypoint))
        {
            _currentWaypointIndex++;
        }
    }

    void HandleMovementReset()
    {
        _currentWaypointIndex = 0;
    }

    void MoveToWaypoint(Waypoint waypoint)
    {
        if (!waypoint) return;

            // Check which direction we need to move in
        if (_currentWaypointIndex >= waypoints.Count)
            return;

        // Check which direction we need to move in
        GridMovementController.Direction dir = GridMovementController.Direction.Left;
        float horizontalDist = waypoint.transform.position.x - transform.position.x;
        float verticalDist = waypoint.transform.position.y - transform.position.y;
        if (Mathf.Abs(horizontalDist) > Mathf.Abs(verticalDist))
        {
            dir = horizontalDist > 0 ? GridMovementController.Direction.Right : GridMovementController.Direction.Left;
        }
        else
        {
            dir = verticalDist > 0 ? GridMovementController.Direction.Up : GridMovementController.Direction.Down;
        }
        _crackmanAnimator.SetFacing(dir);
        gridMovementController.GoAdjacent(dir);
    }
    
    bool IsAtWaypoint(Waypoint waypoint)
    {
        // check if we are close enough to the waypoint
        return Vector3.Distance(
            transform.position, 
            waypoint.transform.position) < epsilon;
    }
}
