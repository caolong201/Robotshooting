
using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] bool ignorYAxis = false;

    private void Start()
    {
        GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        
        if (ignorYAxis)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        }
        else
        {
            transform.position = target.position;
        }
    }
}