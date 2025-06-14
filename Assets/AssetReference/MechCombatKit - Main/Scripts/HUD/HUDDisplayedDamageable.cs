﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VSX.UniversalVehicleCombat;
using System;

public class HUDDisplayedDamageable : MonoBehaviour
{
    [SerializeField] bool isPlayer = false;
    [SerializeField] Slider hpSlider;
    [SerializeField] protected string damageableID;

    public string DamageableID
    {
        get { return damageableID; }
    }

    protected Damageable connectedDamageable;

    [SerializeField] protected GameObject visualElements;

    [SerializeField] protected List<Image> images = new List<Image>();

    public Gradient healthColorGradient;

    public Color destroyedColor = new Color(0f, 0f, 0f, 0.33f);

    [Header("Events")] public UnityEvent onDamaged;

    protected UnityAction<HealthEffectInfo> onDamagedAction;

    private void Start()
    {
        if (hpSlider != null)
        {
            hpSlider.value = 1;
        }
    }

    protected virtual void Reset()
    {
        visualElements = gameObject;

        healthColorGradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(new Color(1, 0.1f, 0.1f, 1f), 0f),
            new GradientColorKey(new Color(1, 0.75f, 0.25f, 1f), 1f)
        };
        healthColorGradient.colorKeys = colorKeys;

        images = new List<Image>(transform.GetComponentsInChildren<Image>());
    }

    public void Connect(Damageable damageable)
    {
        Disconnect();
        connectedDamageable = damageable;
        onDamagedAction = delegate { OnDamaged(); };
        damageable.onDamaged.AddListener(onDamagedAction);
    }

    public void Disconnect()
    {
        if (connectedDamageable != null)
        {
            connectedDamageable.onDamaged.RemoveListener(onDamagedAction);
        }
    }

    protected void OnDamaged()
    {
        onDamaged.Invoke();
        float healthFraction = connectedDamageable.HealthCapacity == 0
            ? 0
            : (connectedDamageable.CurrentHealth / connectedDamageable.HealthCapacity);

        if (hpSlider != null) hpSlider.value = healthFraction;

        if (isPlayer && connectedDamageable.CurrentHealth <= 0)
        {
            UIManager.Instance.ShowEndGame(false);
            return;
        }

        if (isPlayer && healthFraction < 0.2f) //20% HP
        {
            UIManager.Instance.OnHPLow();
        }

        if (isPlayer)
        {
            UIManager.Instance.OnTakeDamage();
        }
    }

    private void Update()
    {
        // if (connectedDamageable == null)
        // {
        //     if (visualElements != null) visualElements.SetActive(false);
        //     return;
        // }
        //
        // visualElements.SetActive(true);
        //
        // float healthFraction = connectedDamageable.HealthCapacity == 0
        //     ? 0
        //     : (connectedDamageable.CurrentHealth / connectedDamageable.HealthCapacity);
        //
        // for (int i = 0; i < images.Count; ++i)
        // {
        //     if (connectedDamageable.Destroyed)
        //     {
        //         images[i].color = destroyedColor;
        //     }
        //     else
        //     {
        //         images[i].color = healthColorGradient.Evaluate(healthFraction);
        //     }
        // }
    }
}