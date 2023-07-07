using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackmanMovement : MonoBehaviour
{
    public List<Waypoint> waypoints;
    int _currentWaypointIndex = 0;
    Waypoint CurrentWaypoint => waypoints[_currentWaypointIndex];
    public float speed = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        MoveToWaypoint(waypoints[0]);
    }
    
    void FixedUpdate()
    {
        if (_currentWaypointIndex >= waypoints.Count)
        {
            return;
        }
        
        MoveToWaypoint(CurrentWaypoint);
        
        if (IsAtWaypoint(CurrentWaypoint))
        {
            _currentWaypointIndex++;
        }

    }

    void MoveToWaypoint(Waypoint waypoint)
    {
        // move toward next waypoint
        transform.position = Vector3.MoveTowards(
            transform.position, 
            waypoint.transform.position, 
            speed * Time.deltaTime);
    }
    
    bool IsAtWaypoint(Waypoint waypoint)
    {
        // check if we are close enough to the waypoint
        return Vector3.Distance(
            transform.position, 
            waypoint.transform.position) < 0.1f;
    }
}
