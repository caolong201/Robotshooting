
using System;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    [SerializeField] CoolingButton coolingButton;
    [SerializeField] private GameObject weaponUIInfo;

    private bool isReloading = false;
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        GameManager.Instance.IsGunReloading = false;
        isReloading = false;
        OnOverheat(false);
        weaponUIInfo.SetActive(true);
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
            weaponUIInfo.SetActive(true);
            isReloading = false;
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
                weaponUIInfo.SetActive(false);
            }
        }
    }

}