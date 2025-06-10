using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutePatrol : MonoBehaviour
{
    public Transform[] waypoints;

    public Transform GetWaypoint(int index)
    {
        if (waypoints == null || waypoints.Length == 0) return null;
        return waypoints[index % waypoints.Length];
    }
    public int GetLength()
    {
        return waypoints != null ? waypoints.Length : 0;
    }
}
