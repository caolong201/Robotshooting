using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TransitionWave : MonoBehaviour
{
    Action _callback;

    public void StartTransition(Transform targetMove, Action callback)
    {
        this._callback = callback;
        Vector3 direction = (targetMove.position - transform.position).normalized;
        direction.y = 0;

        float targetY = Quaternion.LookRotation(direction).eulerAngles.y;
        Vector3 currentEuler = transform.eulerAngles;
        Vector3 finalEuler = new Vector3(0f, targetY, 0f);
        float angleDiff = Mathf.DeltaAngle(currentEuler.y, targetY);
        float adjustedY = currentEuler.y + angleDiff;
        finalEuler.y = adjustedY;


        transform.DORotate(finalEuler, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            transform.DOMove(targetMove.position, 3).SetEase(Ease.Linear).OnComplete(() =>
            {
                var targetEuler = new Vector3(0, targetMove.eulerAngles.y, 0);
                transform.DORotate(targetEuler, .5f).SetEase(Ease.OutSine).OnComplete(() => { _callback?.Invoke(); });
            });
        });
    }
}