using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionWave : MonoBehaviour
{
    private bool _isStart = false;
    private Vector3 _target;
    private Quaternion targetRotation;
    Action _callback;
    
    public void StartTransition(Vector3 start, Vector3 target, Action callback)
    {
        Debug.LogError(start);
        Debug.LogError(target);
        gameObject.SetActive(true);
        this._callback = callback;
        transform.position = start;
        this._target = target;
        _isStart = true;
        UpdateLookTarget();
    }

    private void Update()
    {
        if(!_isStart) return;
        
        transform.position += transform.forward * 40 * Time.deltaTime;
        Debug.Log((Vector3.Distance(transform.position, _target)));
        if (Vector3.Distance(transform.position, _target) < 2)
        {
            _isStart = false;
            _callback?.Invoke();
            gameObject.SetActive(false);
        }
    }
    
    void UpdateLookTarget()
    {
        Vector3 direction = (_target - transform.position).normalized;
        direction.y = 0; // Ignore vertical difference

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
