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
    
    
    public void StartTransition(Vector3 start, Vector3 target, Quaternion camRot,Action callback)
    {
        this._callback = callback;
        transform.position = start;
        this._target = target;
        
        Vector3 direction = (_target - transform.position).normalized;
        direction.y = 0;

        targetRotation = camRot;
       
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 targetEuler = new Vector3(0, lookRotation.eulerAngles.y, 0);
        transform.DORotate(targetEuler, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(target, 2).SetEase(Ease.Linear).OnComplete(() =>
            {
                targetEuler = new Vector3(0, camRot.eulerAngles.y, 0);
                transform.DORotate(targetEuler, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _callback?.Invoke();
                });
            });
        });
    }
}
