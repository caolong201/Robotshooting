using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheatComponent : MonoBehaviour
{
   [SerializeField] CanvasGroup group;
    void Start()
    {
        PlayerAutoFire.Instance.onOverheat += OnOverheat;
    }

    private void OnOverheat(bool isOverheat)
    {
        Debug.Log("OnOverheat: " + isOverheat);
        group.alpha = isOverheat ? 1 : 0;
    }

    private void OnDestroy()
    {
        PlayerAutoFire.Instance.onOverheat -= OnOverheat;
    }
}
