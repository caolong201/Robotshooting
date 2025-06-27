using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReloadButton : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private MachineGun gun;
    private bool isShowing = false;

    private void Update()
    {
        if (GameManager.Instance.IsGunReloading)
        {
            root.SetActive(false);
            return;
        }

        root.SetActive(gun.currentFireTime < gun.fireTime);
    }

    public void OnbtnReloadClicked()
    {
        GameManager.Instance.IsGunReloading = true;
    }
}