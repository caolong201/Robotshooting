using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRayDrawer : MonoBehaviour
{
    public Transform target;
    
    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
            
           // Debug.Log("Dis: " + Vector3.Distance(target.position, transform.position));
        }
    }
}