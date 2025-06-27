
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainPanel : MonoBehaviour
{
    
    [SerializeField] CoolingButton coolingButton;
    [SerializeField] private GameObject btnFire;
    private bool isReloading = false;
    
    public UnityEvent GunReloadDone;
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        GameManager.Instance.IsGunReloading = false;
        isReloading = false;
        OnOverheat(false);
        btnFire.SetActive(true);
    }

    private void OnOverheat(bool isOverheat)
    {
        coolingButton.Show(isOverheat);
    }

    public void OnbtnCoolDownClicked()
    {
        coolingButton.BeginTimer(0, () =>
        {
            GameManager.Instance.IsGunReloading = false;
            OnOverheat(false);
            btnFire.SetActive(true);
            isReloading = false;
            GunReloadDone?.Invoke();
        });
    }

    private void Update()
    {
        if (GameManager.Instance.IsGunReloading)
        {
            if (!isReloading)
            {
                isReloading = true;
                OnOverheat(true);
                OnbtnCoolDownClicked();
                btnFire.SetActive(false);
            }
        }
    }

}