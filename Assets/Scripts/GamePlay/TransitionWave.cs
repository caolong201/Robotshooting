using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TransitionWave : MonoBehaviour
{
    private bool isCanMOve = false;
    private bool isCanRotateCam = false;
    private Vector3 _target;
    private Quaternion targetRotation;
    Action _callback;
    
    [SerializeField] Transform cameraTransform;
    
    public void StartTransition(Vector3 start, Vector3 target, Quaternion camRot,Action callback)
    {
        gameObject.SetActive(true);
        this._callback = callback;
        transform.position = start;
        this._target = target;
        
        Vector3 direction = (_target - transform.position).normalized;
        direction.y = 0; // Ignore vertical difference
        transform.rotation = Quaternion.LookRotation(direction);

        targetRotation = camRot;
        
        isCanMOve = true;
        isCanRotateCam = false;
    }

    private void Update()
    {
        if (isCanMOve)
        {
            UpdateMoveToTarget();
        }

        if (isCanRotateCam)
        {
            UpdateLookTarget();
        }

    }
    
    void UpdateLookTarget()
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            50 * Time.deltaTime
        );
        
        float angleDifference = Quaternion.Angle(transform.rotation , targetRotation);
        if (angleDifference < 0.01f) // small threshold for precision
        {
            Debug.Log("Rotations are the same (within tolerance).");
            isCanRotateCam = false;
            _callback?.Invoke();
            gameObject.SetActive(false);
        }
    }
    
    void UpdateMoveToTarget()
    {
        transform.position += transform.forward * 40 * Time.deltaTime;
        if (Vector3.Distance(transform.position, _target) < 2)
        {
            isCanMOve = false;
            isCanRotateCam = true;
        }
    }
}
